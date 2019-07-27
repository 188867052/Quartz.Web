using System;
using System.Collections.Generic;

namespace Quartz.Entity
{
    public partial class QuartzCronTriggers
    {
        public string SchedName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public string CronExpression { get; set; }

        public string TimeZoneId { get; set; }

        public QuartzTriggers QuartzTriggers { get; set; }
    }
}
