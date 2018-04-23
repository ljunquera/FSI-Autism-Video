using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autism_Video_API.Models.PathfinderEvent;
using Autism_Video_API.Models.PathfinderEvents;

namespace Autism_Video_API.Controllers
{
    public class DataController : ApiController
    {
        // GET api/values
        public IEnumerable<PathfinderEvents> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        public void Post([FromBody]PathfinderEvent data)
        {
        }

    }
}
