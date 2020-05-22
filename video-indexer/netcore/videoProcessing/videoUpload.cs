using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.KeyVault;
using System.Net.Http;
using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;
using videoProcessing;
using System.Text.Json;

namespace videoUpload
{
    public static class videoUpload
    {
        // Http client used in connection to key vault 
        private static HttpClient kvhttpClient = new HttpClient();
        // Helper for CosmosDb operations
        private static CosmosDBHelper cosmosDb;
    
        [FunctionName("videoUpload")]
        public static async Task RunAsync([BlobTrigger("videos/{name}", 
            Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            // Grab Api Key for Video Indexer: https://medium.com/statuscode/getting-key-vault-secrets-in-azure-functions-37620fd20a0b
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var kvClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)
                , kvhttpClient);
            string videoindexerKey = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-video-indexer-key"))).Value;
            var accountId = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-video-indexer-accountid"))).Value;

            // Upload video from blob storage to Video Indexer 
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false; 
            var viClient = new HttpClient(handler);
            viClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", videoindexerKey);
            var apiUrl = "https://api.videoindexer.ai";
            var location = "trial";

            // Get Auth Token for Account 
            var accountAccessTokenRequestResult = viClient.GetAsync($"{apiUrl}/auth/{location}/Accounts/{accountId}/AccessToken?allowEdit=true").Result;
            var accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            // Remove API Key for operation calls 
            viClient.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            // Use the stream blob for the Multipart upload 
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(myBlob), "Video", name);

            var result = viClient.PostAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos?accessToken={accountAccessToken}&name={name}&privacy=private", content).Result;
            var json = result.Content.ReadAsStringAsync().Result;

            // Set variables for Cosmosdb Object
            var myVideoUploadStatus = JsonSerializer.Deserialize<videoIndexData>(json);
            myVideoUploadStatus.state = "Processing";
            myVideoUploadStatus.blobPath = $"/videos/{name}";

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes\nBreakdownId:{myVideoUploadStatus.id}");

            // Connect to Cosmos Db
            CosmosDBHelper.AccessKey = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-cosmosdb-key"))).Value;
            CosmosDBHelper.EndpointUri = (await kvClient.GetSecretAsync(Environment.GetEnvironmentVariable("wm-cosmosdb-uri"))).Value;
            CosmosDBHelper.DatabaseName = "wm-videos";
            CosmosDBHelper.CollectionName = "videos";

            cosmosDb = await CosmosDBHelper.BuildAsync();
            log.LogInformation("Connected to CosmosDb..");

            // Write to Cosmos DbW
            try
            {
                var existing = await cosmosDb.FindDocumentByIdAsync<videoIndexData>(myVideoUploadStatus.id);
                if (existing == null)
                {
                    log.LogInformation($"id {myVideoUploadStatus.id} does not exist in CosmosDb. Creating new record..");
                    await cosmosDb.CreateDocumentIfNotExistsAsync(myVideoUploadStatus, myVideoUploadStatus.id);
                }
                else
                {
                    log.LogInformation($"id {myVideoUploadStatus.id} already exists in CosmosDb. Attempting an update..");
                    await cosmosDb.UpdateDocumentAsync(myVideoUploadStatus, myVideoUploadStatus.id);
                }
            }
            catch (Exception e)
            {
                log.LogInformation($"Exception caught:{e}");
            }
        }
    }
}