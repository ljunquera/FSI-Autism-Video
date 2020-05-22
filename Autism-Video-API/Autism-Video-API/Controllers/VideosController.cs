using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autism_Video_API.Models;

namespace Autism_Video_API.Controllers
{
    public class VideosController: ApiController
    {
        // GET api/videos?patientId=<patientid>&startTime=<startime>&endTime=<endTime>
        public IEnumerable<PathfinderVideo> Get(string patientId, string startTime, string endTime)
        {
            if (patientId != null && startTime != null && endTime != null)
            {
                var pv = new PathfinderVideos(patientId, startTime, endTime, GetStorConnStr());
                return pv.Videos;
            }
            throw new Exception("Invalid Query Options");
        }

        // GET api/videos?patientId=<patientid>
        public IEnumerable<PathfinderVideo> Get(string patientId)
        {
            if (patientId != null)
            {
                var pv = new PathfinderVideos(patientId, GetStorConnStr());
                return pv.Videos;
            }
            throw new Exception("Invalid Query Options");
        }

        // POST api/values
        public string Post([FromBody]PathfinderVideo video)
        {
           return video.Save(GetStorConnStr());
        }

        // PUT
        public void Put(string patientId, string startTime, string url)
        {
            var pv = new PathfinderVideo();
            pv.Update(patientId, startTime, url, GetStorConnStr());
        }

        private string GetStorConnStr()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"];
        }
    }
}