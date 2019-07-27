using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MaterialUI.Entity
{
    public partial class QuartzLog
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreateTime { get; set; }

        public LogLevel LogLevel { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }
    }
}
