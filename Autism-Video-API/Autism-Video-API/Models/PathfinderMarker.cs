using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Autism_Video_API.Models
{
    public class PathfinderMarker
    {
        public string PatientID { get; set; }
        public string RowKey { get; set; }
        public string MarkerTime { get; set; }
        public string Tag { get; set; }


        internal void AMSPublish(string storageConnectionString, string blobConnectionString)
        {
            //ToDo Connect to blob to generate uri
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference("testpublic");
            container.CreateIfNotExists();
            
            //Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(this.FileName);

            // Mocking the call to Media Service

            //Update the media service url in database
            Update(this.PatientID, this.RowKey, blob.Uri.ToString(),this.Tag, this.MarkerTime, storageConnectionString);
        }

        public string FileName { get; set; }



        public PathfinderMarker(){ }

        public PathfinderMarker(string patientID,string rowKey, string markerTime, string tag, string fileName) 
        {
            this.PatientID = patientID;
            this.RowKey = rowKey;
            this.MarkerTime = markerTime;
            this.Tag = tag;
            this.FileName = fileName;
        }


        public void Save(string partitionKey, string rowKey, string markerTime, string tag, string fileName, string storageConnectionString)
        {
            new MarkerEntity(partitionKey, rowKey, markerTime,  tag, fileName, storageConnectionString);

        }

        public void Update(string PatientID, string RowKey, string Url, string Tag,string markerTime, string StorageConnectionString)
        {
            var me = new MarkerEntity();
            me.Update(PatientID, RowKey, Url, Tag, markerTime, StorageConnectionString);
        }
    }
}
