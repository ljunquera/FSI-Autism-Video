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
        // GET api/values/5
        public IEnumerable<PathfinderVideo> Get(string patientId, string startTime, string endTime)
        {
            if (patientId != null && startTime != null && endTime != null)
            {
                var pv = new PathfinderVideos(patientId, startTime, endTime);
                return pv.Videos;
            }
            throw new Exception("Invalid Query Options");
        }

        // POST api/values
        public void Post([FromBody]PathfinderVideo video)
        {
            video.Save();
        }
    }
}