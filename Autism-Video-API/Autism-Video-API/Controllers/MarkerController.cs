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
    public class MarkerController : ApiController
    {
        // GET api/values/5
        public IEnumerable<PathfinderMarker> Get(string patientId,string filename)
        {
            if (patientId != null && filename != null)
            {
                var pv = new PathfinderMarkers(patientId, filename, GetStorConnStr());
                return pv.Markers;
            }
            throw new Exception("Invalid Query Options");
        }

        // POST api/values
        public void Post([FromBody]PathfinderMarker marker)
        {
           marker.Save(marker.PatientID,marker.RowKey,marker.MarkerTime,marker.Tag,marker.FileName,GetStorConnStr());
        }

        // PUT
        public void Put(string patientId, string rowKey, string url, string tag, string markerTime)
        {
            var pv = new PathfinderMarker();
            pv.Update(patientId, rowKey, url, tag, markerTime, GetStorConnStr());
        }

        private string GetStorConnStr()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"];
        }
    }
}