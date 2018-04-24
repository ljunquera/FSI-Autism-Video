using System;
using System.Collections.Generic;

namespace Autism_Video_API.Models
{
    public class PathfinderVideoUI
    {
        public string FileName { get; set; }
        public string StartTime { get; set; }
        public string Token { get; set; }
        public List<PathfinderEventUI> events;
        public List<PathfinderEventUI> Events
        {
            get
            {
                return this.events;
            }
        }

        public PathfinderVideoUI(string fileName, string startTime, string token, List<PathfinderEventUI> events)
        {
            this.FileName = fileName;
            this.StartTime = startTime;
            this.Token = token;
            this.events = events;
        }
    }
}
