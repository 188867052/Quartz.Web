using System;
using System.Collections.Generic;

namespace Quartz.Entity
{
    /// <summary>
    /// 图标.
    /// </summary>
    public partial class Icon
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 排序.
        /// </summary>
        public int Sort { get; set; }
    }
}
