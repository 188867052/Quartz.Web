namespace MaterialUI.Models
{
    using System;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

    public partial class SchedulePostModel
    {
        public string Name { get; set; }

        public string Group { get; set; }

        public string Url { get; set; }

        public DateTime? StartTime { get; internal set; }

        public DateTime? EndTime { get; internal set; }

        public DateTime? LastExcuteTime { get; internal set; }

        public DateTime? NextExcuteTime { get; internal set; }

        public string Cron { get; internal set; }

        public HttpMethod HttpMethod { get; internal set; }
    }
}