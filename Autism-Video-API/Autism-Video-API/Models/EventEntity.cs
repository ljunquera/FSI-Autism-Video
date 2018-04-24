using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Storage;
using Microsoft.Azure.CosmosDB.Table;


namespace Autism_Video_API.Models
{
    public class EventEntity : TableEntity
    {
        public string Skill { get; set; }
        public string Target { get; set; }
        public string Result { get; set; }
        public string Comments { get; set; }

        public EventEntity()
        {
        }

        public EventEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }

        public EventEntity(string partitionKey, string rowKey, string skill, string target, string result, string comments, string StorageConnectionString)
        {
            string TableName = "Events";
            base.PartitionKey = partitionKey;
            base.RowKey = rowKey;
            this.Skill = skill;
            this.Target = target;
            this.Result = result;
            this.Comments = comments;

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



    }
}