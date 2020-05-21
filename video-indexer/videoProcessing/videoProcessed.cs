using System;
using System.Text;
using System.Net.Http;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Web;
using System.Net;

// This function checks if uploaded videos have been processed by video indexer
namespace videoProcessing
{ 
    public static class videoProcessed
    {
        // Http client used in connection to key vault and video indexer
        private static HttpClient kvhttpClient = new HttpClient();

        // Helper for CosmosDb operations
        private static CosmosDBHelper cosmosDb;

        [FunctionName("videoProcessed")]
        public static async System.Threading.Tasks.Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            // Grab Api Key for Video Indexer 
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback), kvhttpClient);
            string videoindexerKey = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-videoindexer-key"))).Value;
            var accountId = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-videoindexer-accountid"))).Value;

            // Connect to Cosmos Db 
            CosmosDBHelper.AccessKey = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-cosmosdb-key"))).Value;
            CosmosDBHelper.EndpointUri = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-cosmosdb-uri"))).Value;
            CosmosDBHelper.DatabaseName = "wm-videos";
            CosmosDBHelper.CollectionName = "videos";

            cosmosDb = await CosmosDBHelper.BuildAsync();
            log.Info("Connected to CosmosDb..");

            // Connect to video indexer 
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            var viHttpClient = new HttpClient(handler);
            viHttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", videoindexerKey);
            var apiUrl = "https://api.videoindexer.ai";
            var location = "trial";

            // Fetch all video ids on Cosmos where processing state is "Processing" 
            foreach (var doc in cosmosDb.FindMatchingDocuments<videoIndexData>("SELECT * FROM videos where videos.state = 'Processing'"))
            {
                // Check video indexer status of each video id, for completed status
                log.Info($"{JsonConvert.SerializeObject(doc)}");
                if (doc.state == "Processing")
                {
                    viHttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", videoindexerKey); // API Key needed in authentication token request
                    var videoTokenRequestResult = viHttpClient.GetAsync($"{apiUrl}/auth/{location}/Accounts/{accountId}/Videos/{(string)doc.id}/AccessToken?allowEdit=true").Result;
                    var videoAccessToken = videoTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

                    // Api call to video indexer 
                    viHttpClient.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key"); // API Key not required in operations call
                    var videoGetIndexRequestResult = viHttpClient.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/{doc.id}/Index?accessToken={videoAccessToken}&language=English").Result;
                    var videoGetIndexResult = videoGetIndexRequestResult.Content.ReadAsStringAsync().Result;
                    // result.EnsureSuccessStatusCode(); 
                    var state = JsonConvert.DeserializeObject<videoIndexData>(videoGetIndexResult).state;
                    log.Info($"Current Video State: {state}\n");

                    // If state of Processing video is now completed in video indexer; push new document to cosmosdb
                    if (state == "Processed")
                    {
                        videoIndexData indexedVideoData = JsonConvert.DeserializeObject<videoIndexData>(videoGetIndexResult);
                        indexedVideoData.blobPath = doc.blobPath;
                        await cosmosDb.UpdateDocumentAsync(indexedVideoData, indexedVideoData.id);
                        log.Info($"Updating id {indexedVideoData.id}..");
                    }
                }
            }

            log.Info($"C# Timer trigger function executed at: {DateTime.Now}...");
        }
    }
}
