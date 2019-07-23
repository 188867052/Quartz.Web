namespace MaterialKit.Job.Entity
{
    using System;
    using Quartz;

    public class JobBriefInfo
    {
        /// <summary>
        /// 任务名称.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 下次执行时间.
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 上次执行时间.
        /// </summary>
        public DateTime? PreviousFireTime { get; set; }

        /// <summary>
        /// 上次执行的异常信息.
        /// </summary>
        public string LastErrMsg { get; set; }

        /// <summary>
        /// 任务状态.
        /// </summary>
        public TriggerState TriggerState { get; set; }

        /// <summary>
        /// 显示状态.
        /// </summary>
        public string DisplayState
        {
            get
            {
                string state;
                //state=Quartz./*TimeSpanParseRule*/
                switch (this.TriggerState)
                {
                    case TriggerState.Normal:
                        state = "正常";
                        break;
                    case TriggerState.Paused:
                        state = "暂停";
                        break;
                    case TriggerState.Complete:
                        state = "完成";
                        break;
                    case TriggerState.Error:
                        state = "异常";
                        break;
                    case TriggerState.Blocked:
                        state = "阻塞";
                        break;
                    case TriggerState.None:
                        state = "不存在";
                        break;
                    default:
                        state = "未知";
                        break;
                }

                return state;
            }
        }
    }
}
