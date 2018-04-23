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
        public IEnumerable<PathfinderVideo> Get(string patientId, DateTime startTime, DateTime endTime)
        {
            var pv = new PathfinderVideos(patientId, startTime, endTime);

            return pv.Videos;
        }

        // POST api/values
        public void Post([FromBody]string patientId, [FromBody]DateTime startTime, [FromBody]DateTime endTime, [FromBody]string fileName)
        {
            var pf = new PathfinderVideo(patientId, startTime, endTime, fileName);
        }
    }
}