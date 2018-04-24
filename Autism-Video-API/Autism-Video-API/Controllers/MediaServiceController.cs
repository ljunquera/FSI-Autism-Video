using Autism_Video_API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Autism_Video_API.Controllers
{
    public class MediaServiceController : ApiController
    {
        // POST api/values
        public void Post([FromBody]PathfinderVideo video)
        {
            video.AMSPublish(GetStorConnStr(), GetBlobConnStr());
        }

        private string GetStorConnStr()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"];
        }

        private string GetBlobConnStr()
        {
            return ConfigurationManager.AppSettings["BlobConnectionString"];
        }
    }
}
