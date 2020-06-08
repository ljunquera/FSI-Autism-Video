using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using Autism_Video_API.Models;

namespace Autism_Video_API.Controllers
{
    public class PatientsController : ApiController
    {
        // GET api/Patient/<PatientID>
        public IEnumerable<string> Get()
        {
            var pv = new PathfinderVideos(GetStorConnStr());
            return pv.Videos.Select(x => x.PatientID).Distinct();
        }

        private string GetStorConnStr()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"];
        }
    }
}