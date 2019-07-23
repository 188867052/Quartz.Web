using System;
using System.Collections.Generic;

namespace MaterialKit.Entity
{
    public partial class QrtzBlobTriggers
    {
        public string SchedName { get; set; }

        public string TriggerName { get; set; }

        public string TriggerGroup { get; set; }

        public byte[] BlobData { get; set; }

        public QrtzTriggers QrtzTriggers { get; set; }
    }
}
