using System;
using System.Collections.Generic;
using Quartz.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace Quartz.Entity
{
    public partial class QuartzLog
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreateTime { get; set; }

        public LogLevel LogLevel { get; set; }

        public HttpMethod TestData { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }
    }
}
