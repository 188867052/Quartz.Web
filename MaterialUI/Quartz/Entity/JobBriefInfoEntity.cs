namespace Quartz.Job.Entity
{
    using System.Collections.Generic;

    public class JobBriefInfoEntity
    {
        /// <summary>
        /// 任务组名.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 任务信息.
        /// </summary>
        public List<JobBriefInfo> JobInfoList { get; set; } = new List<JobBriefInfo>();
    }
}
