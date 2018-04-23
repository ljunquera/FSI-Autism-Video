using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Storage;
using Microsoft.Azure.CosmosDB.Table;

namespace Autism_Video_API.Models
{
    public class PathfinderVideos
    {
        List<PathfinderVideo> videos;

        public List<PathfinderVideo> Videos
        {
            get
            {
                return this.videos;
            }
        }

        private void ExecuteQuery(TableQuery<VideoEntity> query)
        {
            string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=fsiautismteam2;AccountKey=q0mp6PvwFuMY3pp5BESwHgCEFWCMDt8tZIBHL9i5N0n3XmjqrvXDI28U1W7XNGjcX3xmiuSdjO7VQHuNsv9Ofg==;TableEndpoint=https://fsiautismteam2.table.cosmosdb.azure.com:443/;";
            string TableName = "Videos";
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table. 
            // Persist this http connection in the class
            var table = tableClient.GetTableReference(TableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            var data = table.ExecuteQuery<VideoEntity>(query);
            foreach (var d in data)
            {
                videos.Add(new PathfinderVideo(d.PartitionKey, d.RowKey, d.EndTime, d.FileName));
            }
        }

        public PathfinderVideos (string PatientID, string StartTime, string EndTime)
        {
            videos = new List<PathfinderVideo>();
            TableQuery<VideoEntity> query = new TableQuery<VideoEntity>()
            .Where(
                TableQuery.CombineFilters(
                    (TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PatientID),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, StartTime)
                    )),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("EndTime", QueryComparisons.LessThanOrEqual, EndTime)
                )
            );
            ExecuteQuery(query);
            //TODO: go get videos
        }
    }
}