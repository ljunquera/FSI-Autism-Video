using System;
using System.Globalization;

namespace Autism_Video_API.Models
{
    public class PathfinderEventUI
    {
        public int OffsetSeconds { get; set; }
        public string Skill { get; set; }
        public string Target { get; set; }
        public string Result { get; set; }
        public string Comments { get; set; }

        public PathfinderEventUI(PathfinderEvent Pe, DateTime VideoStartTime)
        {
            this.OffsetSeconds = (DateTime.ParseExact(Pe.TimeStamp,"yyyyMMddHHmmss", CultureInfo.InvariantCulture) - VideoStartTime).Seconds;
            this.Skill = Pe.Skill;
            this.Target = Pe.Target;
            this.Result = Pe.Result;
            this.Comments = Pe.Comments;
        }
    }
}
