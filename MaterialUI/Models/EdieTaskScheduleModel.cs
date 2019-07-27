namespace Quartz.Entity
{
    using System;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using Quartz;
    using Quartz.Enums;
    using Quartz.Job;
    using Quartz.Job.Common;

    public class EdieTaskScheduleModel
    {
        public EdieTaskScheduleModel()
        {
        }

        public EdieTaskScheduleModel(QuartzTriggers item)
        {
            if (item.QuartzCronTriggers != null)
            {
                this.CronExpression = item.QuartzCronTriggers.CronExpression;
                this.TriggerType = TriggerTypeEnum.Cron;
            }

            if (item.QuartzSimpleTriggers != null)
            {
                if (item.QuartzSimpleTriggers.RepeatInterval % 1000000 != 0)
                {
                    this.IntervalTime = (int)item.QuartzSimpleTriggers.RepeatInterval;
                    this.IntervalType = TimeSpanParseRule.Milliseconds;
                }
                else
                {
                    this.IntervalTime = (int)(item.QuartzSimpleTriggers.RepeatInterval / 1000000);
                    this.IntervalType = TimeSpanParseRule.Seconds;
                    if (this.IntervalTime % 60 == 0)
                    {
                        this.IntervalType = TimeSpanParseRule.Minutes;
                        this.IntervalTime /= 60;
                        if (this.IntervalTime % 60 == 0)
                        {
                            this.IntervalType = TimeSpanParseRule.Hours;
                            this.IntervalTime /= 60;
                        }
                    }
                }

                this.TriggerType = TriggerTypeEnum.Simple;
            }

            var jobDetail = SchedulerCenter.Instance.Scheduler.GetJobDetail(new JobKey(item.TriggerName, item.JobGroup)).Result;
            this.Url = jobDetail.JobDataMap.GetString(Constant.RequestUrl);
            bool isNotPaused = item.TriggerState.ToLower() != "paused";
            this.Name = item.TriggerName;
            this.Group = item.JobGroup;
            this.TriggerState = item.TriggerState;
            this.NextFireTime = item.NextFireTime;
            this.PrevFireTime = item.PrevFireTime;
            this.StartTime = item.StartTime;
            this.EndTime = item.EndTime;
            this.IsPaused = !isNotPaused;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string TriggerState { get; set; }

        public string Url { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? NextFireTime { get; set; }

        public DateTime? PrevFireTime { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string CronExpression { get; set; }

        public bool IsPaused { get; }

        public int? IntervalTime { get; set; }

        public TimeSpanParseRule IntervalType { get; set; }

        public TriggerTypeEnum TriggerType { get; set; }

        public string Group { get; set; }

        public string Parameters { get; set; }

        public string Description { get; set; }

        public int? RunTimes { get; set; }

        public string IconClass { get; set; }

        [BindNever]
        public JobKey JobKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.Group))
                {
                    return new JobKey(this.Name);
                }

                return new JobKey(this.Name, this.Group);
            }
        }

        [BindNever]
        public TriggerKey TriggerKey
        {
            get
            {
                if (string.IsNullOrEmpty(this.Group))
                {
                    return new TriggerKey(this.Name);
                }

                return new TriggerKey(this.Name, this.Group);
            }
        }
    }
}
