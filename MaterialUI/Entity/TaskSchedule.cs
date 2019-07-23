using System;
using System.Collections.Generic;
using MaterialUI.Enums;
using Quartz;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace MaterialUI.Entity
{
    public partial class TaskSchedule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TriggerState Status { get; set; }

        public string Url { get; set; }

        public string ExceptionMessage { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? LastExcuteTime { get; set; }

        public DateTime? NextExcuteTime { get; set; }

        public string ExcutePlan { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string CronExpression { get; set; }

        public int? IntervalTime { get; set; }

        public TimeSpanParseRule IntervalType { get; set; }

        public bool IsEnable { get; set; }

        public TriggerTypeEnum TriggerType { get; set; }

        public string Group { get; set; }

        public string Parameters { get; set; }

        public string Description { get; set; }

        public int? RunTimes { get; set; }

        public string IconClass { get; set; }
    }
}
