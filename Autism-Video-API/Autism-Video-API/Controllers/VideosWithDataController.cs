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
    public class VideosWithDataController: ApiController
    {
        // GET api/values/5
        public PathfinderVideoUI Get(string patientId, string startTime, string endTime)
        {
            if (patientId != null && startTime != null && endTime != null)
            {
                var pve = new PathfinderVideos(patientId, startTime, endTime, GetStorConnStr());


                if (pve == null || pve.Videos.Count == 0)
                    return null;


                PathfinderVideoUI pathfinderVideoUI = new PathfinderVideoUI(pve.Videos[0]);

                var pathfinderEvents = new PathFinderEvents(patientId, startTime, endTime, null, null, GetStorConnStr());

                foreach (var pe in pathfinderEvents.Events)
                    {
                    pathfinderVideoUI.Events.Add(new PathfinderEventUI(pe, pathfinderVideoUI.StartTime));
                    }
                    //ToDo: Get the URL and Token for video from Media Services
                return pathfinderVideoUI;
            }
            throw new Exception("Invalid Query Options");
        }

        private string GetStorConnStr()
        {
            return ConfigurationManager.AppSettings["StorageConnectionString"];
        }

    }
}
