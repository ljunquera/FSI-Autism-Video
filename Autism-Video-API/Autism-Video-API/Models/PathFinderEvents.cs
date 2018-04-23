using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Autism_Video_API.Models
{
    public class PathFinderEvents
    {
        List<PathfinderEvent> events;

        public PathFinderEvents(string PatientID, string StartDate, string EndDate)
        {
            events = new List<PathfinderEvent>();

            //TODO: add evemts from DB
        }

        public PathFinderEvents(string PatientID, string StartDate, string EndDate, string Skill)
        {
            events = new List<PathfinderEvent>();

            //TODO: add evemts from DB
        }

        public PathFinderEvents(string PatientID, string StartDate, string EndDate, string Skill, string Target)
        {
            events = new List<PathfinderEvent>();

            //TODO: add evemts from DB
        }




        public List<PathfinderEvent> Events {get;set;}
    }
}