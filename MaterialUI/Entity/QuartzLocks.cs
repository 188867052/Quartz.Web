using System;
using System.Collections.Generic;

namespace Quartz.Entity
{
    public partial class QuartzLocks
    {
        public string SchedName { get; set; }

        public string LockName { get; set; }
    }
}
