using System;
using System.IO;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VideoProcessor
{
    public static class VideoProcessor
    {
       

        [FunctionName("VideoProcessor")]
        public static void Run([BlobTrigger("videos/{name}", Connection = "storageConnectionString")]Stream myBlob, string name,
            [CosmosDB(
                databaseName: "videoUploaderDB",
                collectionName: "videoUploaderDB",
                ConnectionStringSetting = "CosmosDBConnection")]out dynamic document,
            ILogger log)
        {
            // Get credentials and call the vi api for access token 
            string videoindexerKey = Environment.GetEnvironmentVariable("videoIndexerKey");
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            var viClient = new HttpClient(handler);
            viClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", videoindexerKey);
            var apiUrl = "https://api.videoindexer.ai";
            var location = "trial";
            var accountId = Environment.GetEnvironmentVariable("accountId");
            //Get Auth token
            var accountAccessTokenRequestResult = viClient.GetAsync($"{apiUrl}/auth/{location}/Accounts/{accountId}/AccessToken?allowEdit=true").Result;
            var accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            // Remove API Key for operation calls 
            viClient.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            // Use the stream blob for the Multipart upload 
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(myBlob), "Video", name);

            document = "";
            try
            {
                var result = viClient.PostAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos?accessToken={accountAccessToken}&name={name}&privacy=private", content).Result;
                var json = result.Content.ReadAsStringAsync().Result;

                var myVideoUploadStatus = JsonConvert.DeserializeObject<videoIndexData>(json);

                myVideoUploadStatus.state = "Processing";
                myVideoUploadStatus.blobPath = $"/videos/{name}";

                log.LogInformation($"Video Data \n Name:{name} \n Size: {myBlob.Length} Bytes\nBreakdownId:{myVideoUploadStatus.id}");

                while (true)
                {
                    // Api call to video indexer 
                    var videoGetIndexRequestResult = viClient.GetAsync($"{apiUrl}/{location}/Accounts/{accountId}/Videos/{myVideoUploadStatus.id}/Index?accessToken={accountAccessToken}&language=English").Result;
                    var videoGetIndexResult = videoGetIndexRequestResult.Content.ReadAsStringAsync().Result;
                    var state = JsonConvert.DeserializeObject<videoIndexData>(videoGetIndexResult).state;
                    log.LogInformation($"Current Video State: {state}\n");

                    if (state == "Processed")
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(10000);
                }

                //log.LogInformation($" Video Data = {myVideoUploadStatus}");
                document = new { Description = $"Uploaded Video with id:: {myVideoUploadStatus.id}", id = Guid.NewGuid() };

            }
            catch (Exception ex)
            {
                log.LogInformation($"Error executing  blob\n Name:{name} \n Exception: {ex.Data} ");
            }

            

            // Upload video from blob storage to Video Indexer 

            // Video Upload API call 

            // Push video id into cosmos db 

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
