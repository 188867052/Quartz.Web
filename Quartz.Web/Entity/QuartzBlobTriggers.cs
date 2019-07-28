using System;
using System.Collections.Generic;

namespace Quartz.Entity
{
    public partial class QuartzBlobTriggers
    {
        public string SchedName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        /// <summary>
        /// 数据.
        /// </summary>
        public byte[] BlobData { get; set; }

        public QuartzTriggers QuartzTriggers { get; set; }
    }
}
