﻿using System;
using System.Collections.Generic;

namespace Quartz.Entity
{
    public partial class QuartzSchedulerState
    {
        public string SchedName { get; set; }

        public string InstanceName { get; set; }

        public long LastCheckinTime { get; set; }

        public long CheckinInterval { get; set; }
    }
}
