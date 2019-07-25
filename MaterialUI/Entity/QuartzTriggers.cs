using System;
using System.Collections.Generic;

namespace MaterialUI.Entity
{
    public partial class QuartzTriggers
    {
        public string SchedName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public string JobName { get; set; }

        public string JobGroup { get; set; }

        public string Description { get; set; }

        public DateTime? NextFireTime { get; set; }

        public DateTime? PrevFireTime { get; set; }

        public int? Priority { get; set; }

        public string TriggerState { get; set; }

        public string TriggerType { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string CalendarName { get; set; }

        public int? MisfireInstr { get; set; }

        public byte[] JobData { get; set; }

        public QuartzJobDetails QuartzJobDetails { get; set; }

        public QuartzBlobTriggers QuartzBlobTriggers { get; set; }

        public QuartzCronTriggers QuartzCronTriggers { get; set; }

        public QuartzSimpleTriggers QuartzSimpleTriggers { get; set; }

        public QuartzSimpropTriggers QuartzSimpropTriggers { get; set; }
    }
}
