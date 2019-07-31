using System;
using System.Collections.Generic;

namespace Quartz.Entity
{
    public partial class QuartzTriggers
    {
        public string SchedName { get; set; }

        /// <summary>
        /// Trigger名称.
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// Trigger分组.
        /// </summary>
        public string TriggerGroup { get; set; }

        /// <summary>
        /// Job名称.
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Job分组.
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 描述.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 下次执行时间.
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 上次执行时间.
        /// </summary>
        public DateTime? PrevFireTime { get; set; }

        /// <summary>
        /// 优先级.
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// 状态.
        /// </summary>
        public string TriggerState { get; set; }

        /// <summary>
        /// 类型.
        /// </summary>
        public string TriggerType { get; set; }

        /// <summary>
        /// 开始时间.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间.
        /// </summary>
        public DateTime? EndTime { get; set; }

        public string CalendarName { get; set; }

        public int? MisfireInstr { get; set; }

        /// <summary>
        /// 数据.
        /// </summary>
        public byte[] JobData { get; set; }

        public QuartzJobDetails QuartzJobDetails { get; set; }

        public QuartzBlobTriggers QuartzBlobTriggers { get; set; }

        public QuartzCronTriggers QuartzCronTriggers { get; set; }

        public QuartzSimpleTriggers QuartzSimpleTriggers { get; set; }

        public QuartzSimpropTriggers QuartzSimpropTriggers { get; set; }
    }
}
