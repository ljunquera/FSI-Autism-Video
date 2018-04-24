using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autism_Video_API.Models;

namespace Autism_Video_API.Controllers
{
    public class VideosWithDataController: ApiController
    {
        // GET api/values/5
        public IEnumerable<PathfinderVideoUI> Get(string patientId, string startTime, string endTime)
        {
            if (patientId != null && startTime != null && endTime != null)
            {
                VideosController videosController = new VideosController();
                DataController dataController = new DataController();
                IEnumerable<PathfinderVideo> pathfinderVideos = videosController.Get(patientId, startTime, endTime);
                IEnumerable<PathfinderEvent> pathfinderEvents = dataController.Get(patientId, startTime, endTime, null, null);
                List<PathfinderVideoUI> pathfinderVideosUI = new List<PathfinderVideoUI>();
                List<PathfinderEventUI> pathfinderEventsUI = new List<PathfinderEventUI>();
                foreach(var pathfinderVideo in pathfinderVideos)
                {
                    string sTime = pathfinderVideo.StartTime;
                    string eTime = pathfinderVideo.EndTime;
                    var videoEvents = pathfinderEvents.Where(a => DateTime.Parse(a.TimeStamp) >= DateTime.Parse(sTime) && DateTime.Parse(a.TimeStamp) <= DateTime.Parse(eTime));
                    foreach (var videoEvent in videoEvents)
                    {
                        pathfinderEventsUI.Add(new PathfinderEventUI(videoEvent.Comments, (DateTime.Parse(videoEvent.TimeStamp) - DateTime.Parse(sTime)).ToString()));
                    }
                    //ToDo: Get the URL and Token for video from Media Services
                    pathfinderVideosUI.Add(new PathfinderVideoUI(pathfinderVideo.PatientID, pathfinderVideo.StartTime, "", pathfinderEventsUI));
                }
                return pathfinderVideosUI;
            }
            throw new Exception("Invalid Query Options");
        }

    }
}
