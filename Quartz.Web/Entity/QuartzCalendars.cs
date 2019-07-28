using System;
using System.Collections.Generic;

namespace Quartz.Entity
{
    public partial class QuartzCalendars
    {
        public string SchedName { get; set; }

        public string CalendarName { get; set; }

        public byte[] Calendar { get; set; }
    }
}
