using Microsoft.Rest;
using System;
using System.IO;
using System.Net.Http;
using VideoUploader.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Configuration;
using Newtonsoft.Json;

namespace VideoUploader
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            string fileName = args[1];
            string patientId = args[0];

            FileInfo fi = new FileInfo(fileName);

            var name = fi.Name;
            var startTime = fi.CreationTimeUtc;
            var endTime = fi.LastWriteTimeUtc;

            var pathfinderVideo = new PathfinderVideo(patientId, startTime.ToString("yyyyMMddHHmmss"), endTime.ToString("yyyyMMddHHmmss"), name);

            var response = client.PostAsJsonAsync<PathfinderVideo>(
                "http://fsiautismny2.azurewebsites.net/api/Videos", pathfinderVideo);

            response.Result.EnsureSuccessStatusCode();

            var sasUri = new Uri(JsonConvert.DeserializeObject<string>(response.Result.Content.AsString()));

            //open file as stream and post to the uri
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;
            ServicePointManager.Expect100Continue = false;

            CloudBlockBlob blob = new CloudBlockBlob(sasUri);

            // Create operation: Upload a blob with the specified name to the container.
            // If the blob does not exist, it will be created. If it does exist, it will be overwritten.
            blob.UploadFromFile(fileName);
            
            //post to AMSingestion api

            }
    }
}
