using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using Autism_Video_API.Models;

namespace Autism_Video_API.Controllers
{
    public class PatientController : ApiController
    {
        // GET api/Patient/<PatientID>
        public IEnumerable<PathfinderVideo> Get(string patientId)
        {
            if (patientId != null)
            {
                var pv = new PathfinderVideos(patientId, GetStorConnStr());
                return pv.Videos;
            }
            throw new Exception("Invalid Query Options");
        }

        private string GetStorConnStr()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"];
        }
    }
}