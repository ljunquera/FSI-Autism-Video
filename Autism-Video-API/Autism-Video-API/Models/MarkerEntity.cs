using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Storage;
using Microsoft.Azure.CosmosDB.Table;

namespace Autism_Video_API.Models
{
    public class MarkerEntity : TableEntity
    {
        public string MarkerTime { get; set; }
        public string Tag { get; set; }
        public string FileName { get; set; }

        public MarkerEntity()
        {
        }

        public MarkerEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }

         public MarkerEntity(string partitionKey, string rowKey, string markerTime, string tag, string fileName, string StorageConnectionString)
        {
            string TableName = "Markers";
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
            this.MarkerTime = markerTime;
            this.Tag = tag;
            this.FileName = fileName;

            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table. 
            // Persist this http connection in the class
            var table = tableClient.GetTableReference(TableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            var io = TableOperation.Insert(this);

            table.Execute(io);

        }

        public void Update(string partitionKey, string rowKey, string url, string tag, string markerTime, string StorageConnectionString)
        {
            string TableName = "Markers";

            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table. 
            // Persist this http connection in the class
            var table = tableClient.GetTableReference(TableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            TableOperation retrieveOperation = TableOperation.Retrieve<MarkerEntity>(partitionKey, rowKey);
            TableResult retrievedResult = table.Execute(retrieveOperation);
            MarkerEntity updateEntity = (MarkerEntity)retrievedResult.Result;
            if (updateEntity != null)
            {
                //Change the description
                updateEntity.FileName = url;
                updateEntity.Tag = tag;
                updateEntity.MarkerTime = markerTime;

                // Create the InsertOrReplace TableOperation
                TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);

                // Execute the operation.
                table.Execute(insertOrReplaceOperation);
            }
        }
    }
}