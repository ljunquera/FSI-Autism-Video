using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autism_Video_API.Models;
using System.Configuration;

namespace Autism_Video_API.Controllers
{
    public class DataController : ApiController
    {
        // GET api/values
        public IEnumerable<PathfinderEvent> Get(string PatientId, string StartDate, string EndDate, string Skill, string Target )
        {
            if (Skill != null && Target != null)
            {
                var pve = new PathFinderEvents(PatientId, StartDate, EndDate, Skill, Target, GetStorConnStr());
                return pve.Events;
            }
            else
            {
                if (Skill != null)
                {
                    var pve = new PathFinderEvents(PatientId, StartDate, EndDate, Skill, GetStorConnStr());
                    return pve.Events;
                }
                else
                {
                    var pve = new PathFinderEvents(PatientId, StartDate, EndDate, GetStorConnStr());
                    return pve.Events;
                }
            }
            //invalid query
            throw new Exception("Invalid Query Options");
        }

        // POST api/values
        public void Post([FromBody]PathfinderEvent pathfinderData)
        {
            pathfinderData.Save(GetStorConnStr());
        }

        private string GetStorConnStr()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"]; 
        }

    }
}
