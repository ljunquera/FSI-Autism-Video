using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Storage;
using Microsoft.Azure.CosmosDB.Table;
namespace Autism_Video_API.Models
{

    public class PathfinderMarkers
    {
        List<PathfinderMarker> markers;

        public List<PathfinderMarker> Markers
        {
            get
            {
                return this.markers;
            }
        }

        private void ExecuteQuery(TableQuery<MarkerEntity> query, string StorageConnectionString)
        {
            markers = new List<PathfinderMarker>();
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

            var data = table.ExecuteQuery<MarkerEntity>(query);
            foreach (var d in data)
            {
                markers.Add(new PathfinderMarker(d.PartitionKey, d.RowKey, d.MarkerTime, d.Tag, d.FileName));
            }
        }

        public PathfinderMarkers(string PatientID, string fileName, string StorageConnectionString)
        {
            TableQuery<MarkerEntity> query = new TableQuery<MarkerEntity>()
            .Where(
                    (TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PatientID),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("FileName", QueryComparisons.GreaterThanOrEqual, fileName)
                    )
            ));
            ExecuteQuery(query, StorageConnectionString);
            //TODO: go get videos
        }
    }
}