namespace MaterialUI.Job.Entity
{
    using System.Collections.Generic;

    public class JobInfoEntity
    {
        /// <summary>
        /// 任务组名.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 任务信息.
        /// </summary>
        public List<JobInfo> JobInfoList { get; set; } = new List<JobInfo>();
    }
}
