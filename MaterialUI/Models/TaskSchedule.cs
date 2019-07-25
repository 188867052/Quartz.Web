namespace MaterialUI.Entity
{
    using System;
    using MaterialUI.Enums;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using Quartz;

    public partial class TaskScheduleModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TriggerState Status { get; set; }
        public string TriggerState { get; set; }

        public string Url { get; set; }

        public string ExceptionMessage { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? NextFireTime { get; set; }

        public DateTime? PrevFireTime { get; set; }

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
