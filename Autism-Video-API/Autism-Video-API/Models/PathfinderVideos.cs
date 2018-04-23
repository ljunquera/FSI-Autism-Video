using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autism_Video_API.Models
{
    public class PathfinderVideos
    {
        List<PathfinderVideo> videos;

        public List<PathfinderVideo> Videos { get; set; }

        public PathfinderVideos (string PatientID, DateTime StartTime, DateTime EndTime)
        {
            videos = new List<PathfinderVideo>();
            //TODO: go get videos
        }
    }
}