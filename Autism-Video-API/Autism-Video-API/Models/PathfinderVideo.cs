using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autism_Video_API.Models
{
    public class PathfinderVideo
    {
        public string PatientID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string FileName { get; set; }
    
        public PathfinderVideo(string PatientID, DateTime StartTime, DateTime EndTime, string FileName) 
        {
            //ToDo: Persist the meta data about video
        }
    }
}
