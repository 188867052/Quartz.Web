namespace Quartz.Models
{
    using System;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

    public partial class SchedulePostModel
    {
        public string Name { get; set; }

        public string Group { get; set; }

        public string Url { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? LastExcuteTime { get; set; }

        public DateTime? NextExcuteTime { get; set; }

        public string Cron { get; set; }

        public HttpMethod HttpMethod { get; set; }
    }
}