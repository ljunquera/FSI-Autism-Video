using System;
namespace Autism_Video_API.Models
{
    public class PathfinderEventUI
    {
        public string Text { get; set; }
        public string RelativeTime { get; set; }

        public PathfinderEventUI(string text, string relativeTime)
        {
            this.Text = text;
            this.RelativeTime = relativeTime;
        }
    }
}
