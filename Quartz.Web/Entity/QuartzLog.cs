using System;
using System.Collections.Generic;
using Quartz.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace Quartz.Entity
{
    /// <summary>
    /// 日志表.
    /// </summary>
    public partial class QuartzLog
    {
        /// <summary>
        /// 主键.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 触发器名称.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 触发器组.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 日志记录.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 日志等级.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Http请求类型.
        /// </summary>
        public HttpMethod TestData { get; set; }
    }
}
