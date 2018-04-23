using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autism_Video_API.Models
{
    public class PathFinderEvents
    {
        List<PathfinderEvent> events;

        public PathFinderEvents(string PatientID, DateTime StartDate, DateTime EndDate)
        {
            events = new List<PathfinderEvent>();

            //TODO: add evemts from DB
        }

        public PathFinderEvents(string PatientID, DateTime StartDate, DateTime EndDate, string Skill)
        {
            events = new List<PathfinderEvent>();

            //TODO: add evemts from DB
        }

        public PathFinderEvents(string PatientID, DateTime StartDate, DateTime EndDate, string Skill, string Target)
        {
            events = new List<PathfinderEvent>();

            //TODO: add evemts from DB
        }

        public List<PathfinderEvent> Events {get;set;}
    }
}