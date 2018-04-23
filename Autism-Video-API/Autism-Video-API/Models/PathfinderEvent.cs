using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autism_Video_API.Models
{
    public class PathfinderEvent
    {
        public string PatientID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Skill { get; set; }
        public string Target { get; set; }
        public string Result { get; set; }
        public string Comments { get; set; }

        public void Save()
        {
            var ee = new EventEntity(PatientID, TimeStamp.ToString("yyyyMMddHHmmss"), Skill, Target, Result, Comments);
        }
    }
}