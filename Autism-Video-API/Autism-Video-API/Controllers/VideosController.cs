using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autism_Video_API.Models;

namespace Autism_Video_API.Controllers
{
    public class VideosController: ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public IEnumerable<PathfinderVideo> Get(int patientId, string startTime, string endTime)
        {
            return new PathfinderVideos(patientId, startTime, endTime);
        }

        // POST api/values
        public void Post([FromBody]string patientId, [FromBody]string startTime, [FromBody]string endTime, [FromBody]string fileName)
        {
            PathfinderVideo pfVideo = new PathfinderVideo();
            pfVideo.AddVideoToPathfinderVideo(patientID, startTime, endTime, fileName);
        }
    }
}