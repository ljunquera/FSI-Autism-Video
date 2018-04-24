using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autism_Video_API.Models
{
    public class PathfinderVideo
    {
        public string PatientID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string FileName { get; set; }
        public string MediaServiceUrl { get; set; }

        public PathfinderVideo(){ }

        public PathfinderVideo(string patientID, string startTime, string endTime, string fileName) 
        {
            this.PatientID = patientID;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.FileName = fileName;
            this.MediaServiceUrl = "";
        }

        public void Save(string StorageConnectionString)
        {
            var ve = new VideoEntity(PatientID, StartTime, EndTime, FileName, StorageConnectionString);
        }

        public void Update(string PatientID, string StartTime, string Url, string StorageConnectionString)
        {
            var ve = new VideoEntity();
            ve.Update(PatientID, StartTime, Url, StorageConnectionString);
        }
    }
}
