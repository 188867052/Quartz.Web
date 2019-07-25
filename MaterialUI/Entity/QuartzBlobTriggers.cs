using System;
using System.Collections.Generic;

namespace MaterialUI.Entity
{
    public partial class QuartzBlobTriggers
    {
        public string SchedName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public byte[] BlobData { get; set; }

        public QuartzTriggers QuartzTriggers { get; set; }
    }
}
