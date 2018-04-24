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
    
        public PathfinderVideo(string patientID, string startTime, string endTime, string fileName) 
        {
            this.PatientID = patientID;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.FileName = fileName;
        }

        public void Save()
        {
            var ve = new VideoEntity(PatientID, StartTime, EndTime, FileName);
        }
    }
}
