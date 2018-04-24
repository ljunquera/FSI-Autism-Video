using System;
using System.Collections.Generic;
using System.Globalization;

namespace Autism_Video_API.Models
{
    public class PathfinderVideoUI
    {
        public string URL { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PatientId { get; set; }


        public List<PathfinderEventUI> Events;

       

        public PathfinderVideoUI(PathfinderVideo pv)
        {
            this.URL = pv.MediaServiceUrl;
            this.StartTime = DateTime.ParseExact(pv.StartTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            this.EndTime = DateTime.ParseExact(pv.EndTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            this.PatientId = pv.PatientID;
            this.Events = new List<PathfinderEventUI>();
        }
    }
}
