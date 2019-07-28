namespace Quartz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AspNetCore.Extensions;
    using global::Dapper;
    using MaterialUI.Framework.Filter;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using Quartz;
    using Quartz.DataShapes;
    using Quartz.Entity;
    using Quartz.Enums;
    using Quartz.Framework;
    using Quartz.GridConfiguration;
    using Quartz.GridConfigurations.Schedule;
    using Quartz.Impl.Matchers;
    using Quartz.Impl.Triggers;
    using Quartz.Job;
    using Quartz.Job.Common;
    using Quartz.Job.Entity;
    using Quartz.Models;
    using Quartz.SearchFilterConfigurations;
    using Quartz.ViewConfiguration.Schedule;
    using Serilog;

    public class ScheduleController : StandardController
    {
        public ScheduleController(QuartzContext dbContext)
            : base(dbContext)
        {
            this.scheduler = SchedulerCenter.Instance.Scheduler;
        }

        public string GetTime()
        {
            return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        public IActionResult Index()
        {
            using (this.DbContext)
            {
                QuartzTriggersDataShape.Index(this.DbContext);
                var index = 1;
                var size = 10;
                var list = this.DbContext.QuartzTriggers.PageToList(index, size, out int total);
                List<TaskScheduleModel> models = list.Select(item => new TaskScheduleModel(item)).ToList();
                var page = new SchedualGridSearch<TaskScheduleModel, SchedulePostModel>(models, index, size, total);
                return this.SearchGrid(page);
            }
        }

        public IActionResult GridStateChange(int index, int size)
        {
            QuartzTriggersDataShape.Index(this.DbContext);
            var list = this.DbContext.QuartzTriggers.PageToList(index, size, out int total);

            List<TaskScheduleModel> models = list.Select(item => new TaskScheduleModel(item)).ToList();
            var grid = new SchedualGridConfiguration<TaskScheduleModel>();
            var html = grid.Render(index, size, models, total);

            return this.HtmlResult(TagHelper.ToHtml(html));
        }

        public IActionResult LogGridStateChange(int index, int size, string name, string group)
        {
            var grid = new LogDialogGridConfiguration<QuartzLog>(name, group);
            IQueryable<QuartzLog> query = this.DbContext.QuartzLog.AsNoTracking();
            query = query.AddStringEqualFilter(name, o => o.Name);
            query = query.AddStringEqualFilter(group, o => o.Group);
            query = query.OrderByDescending(o => o.CreateTime);

            IList<QuartzLog> models = query.PageToList(index, size, out int total);
            var responsiveTable = grid.Render(index, size, models, total);
            return this.HtmlResult(TagHelper.ToHtml(responsiveTable));
        }

        public IActionResult LogDialog(string name, string group, int index, int size)
        {
            IQueryable<QuartzLog> query = this.DbContext.QuartzLog.Where(o => o.Name == name && o.Group == group).OrderByDescending(o => o.CreateTime);
            IList<QuartzLog> models = query.PageToList(index, size, out int total);
            LogDialog dialog = new LogDialog(models, index, size, total, name, group);
            return this.Dialog(dialog);
        }

        public IActionResult DeleteDialog(string name, string group)
        {
            var trigger = this.DbContext.QuartzTriggers.FirstOrDefault(o => o.TriggerName == name && o.TriggerGroup == group);
            //trigger = DapperExtension.Query<QuartzTriggers>("trigger_name=@name and trigger_group=@group", new { name, group });
            var dialog = new DeleteConfiguration(trigger);
            return this.Dialog(dialog);
        }

        public IActionResult Edit(string name, string group)
        {
            QuartzTriggersDataShape.Index(this.DbContext);
            var trigger = this.DbContext.QuartzTriggers.FirstOrDefault(o => o.TriggerName == name && o.TriggerGroup == group);

            var dialog = new EditScheduleDialog<EdieTaskScheduleModel, EdieTaskScheduleModel>(new EdieTaskScheduleModel(trigger));
            return this.Dialog(dialog);
        }

        public IActionResult ReplaceColumn(TriggerTypeEnum type)
        {
            var entity = new EdieTaskScheduleModel
            {
                TriggerType = type,
            };
            var replaceColumn = new ReplaceLargeColumn<EdieTaskScheduleModel, EdieTaskScheduleModel>(entity);
            var replace = replaceColumn.Render();
            return this.HtmlResult(replace);
        }

        /// <summary>
        /// 查询任务.
        /// </summary>
        [HttpPost]
        public async Task<ScheduleEntity> QueryJob([FromBody]JobKey job)
        {
            return await this.QueryJobAsync(job.Group, job.Name);
        }

        /// <summary>
        /// 立即执行.
        /// </summary>
        [HttpPost]
        public async Task<bool> TriggerJob([FromBody]JobKey job)
        {
            await this.scheduler.TriggerJob(job);
            return true;
        }

        /// <summary>
        /// 暂停任务.
        /// </summary>
        public async Task StopJob(string name, string group)
        {
            await this.scheduler.PauseJob(new JobKey(name, group));
        }

        /// <summary>
        /// 恢复运行暂停的任务.
        /// </summary>
        public async Task ResumeJob(string name, string group)
        {
            var jobKey = new JobKey(name, group);
            bool isExists = await this.scheduler.CheckExists(jobKey);
            if (isExists)
            {
                await this.scheduler.ResumeJob(jobKey);
            }
            else
            {
                throw new Exception("操作错误");
            }
        }

        public IActionResult AddDialog()
        {
            var dialog = new EditScheduleDialog<EdieTaskScheduleModel, EdieTaskScheduleModel>();
            return this.Dialog(dialog);
        }

        [HttpPost]
        public IActionResult Search(SchedulePostModel model)
        {
            IQueryable<TaskScheduleModel> query = null;
            query = query.AddStringContainsFilter(o => o.Name, model.Name);
            query = query.AddStringContainsFilter(o => o.Group, model.Group);
            query = query.AddStringContainsFilter(o => o.Url, model.Url);
            query = query.AddStringContainsFilter(o => o.CronExpression, model.Cron);
            query = query.AddDateTimeBetweenFilter(model.StartTime, model.EndTime, o => o.StartTime);
            query = query.AddDateTimeBetweenFilter(model.StartTime, model.EndTime, o => o.EndTime);
            var list = query.ToList();

            list = this.GetAllJobAsync(list).Result;
            ScheduleGridConfiguration<TaskScheduleModel> grid = new ScheduleGridConfiguration<TaskScheduleModel>(list);

            return this.HtmlResult(grid.Render());
        }

        [HttpPost]
        public async Task<BaseResult> ModifyJob(TaskScheduleModel model)
        {
            await this.scheduler.PauseJob(model.JobKey);
            await this.scheduler.DeleteJob(model.JobKey);
            await this.AddScheduleJobAsync(model);

            if (!model.IsPaused)
            {
                await this.scheduler.PauseJob(model.JobKey);
            }

            return new BaseResult() { Msg = "修改计划任务成功！" };
        }

        [HttpGet]
        public async Task<bool> StartSchedule()
        {
            if (this.scheduler.InStandbyMode)
            {
                await this.scheduler.Start();
                Log.Information("任务调度启动！");
            }

            return this.scheduler.InStandbyMode;
        }

        [HttpGet]
        public async Task<bool> StopSchedule()
        {
            if (!this.scheduler.InStandbyMode)
            {
                // TODO  注意：Shutdown后Start会报错，所以这里使用暂停。
                await this.scheduler.Standby();
                Log.Information("任务调度暂停！");
            }

            return !this.scheduler.InStandbyMode;
        }

        /// <summary>
        /// 获取所有Job信息（简要信息 - 刷新数据的时候使用）.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<JobBriefInfoEntity>> GetAllJobBriefInfo()
        {
            return await this.GetAllJobBriefInfoAsync();
        }

        /// <summary>
        /// 移除异常信息.
        /// </summary>
        [HttpPost]
        public async Task<bool> RemoveErrLog([FromBody]JobKey jobKey)
        {
            return await this.RemoveErrLog(jobKey.Group, jobKey.Name);
        }

        /// <summary>
        /// 添加任务.
        /// </summary>
        /// <param name="entity">entity.</param>
        /// <returns>BaseResult.</returns>
        [HttpPost]
        public async Task<BaseResult> AddJob(TaskScheduleModel entity)
        {
            return await this.AddScheduleJobAsync(entity);
        }

        public async Task<JsonResult> Delete(string jobName, string jobGroup)
        {
            await this.scheduler.DeleteJob(new JobKey(jobName, jobGroup));
            return this.Json(1);
        }

        private async Task<ScheduleEntity> QueryJobAsync(string jobGroup, string jobName)
        {
            var entity = new ScheduleEntity();
            var jobKey = new JobKey(jobName, jobGroup);
            var jobDetail = await this.scheduler.GetJobDetail(jobKey);
            var triggersList = await this.scheduler.GetTriggersOfJob(jobKey);
            var triggers = triggersList.AsEnumerable().FirstOrDefault();
            var intervalSeconds = (triggers as SimpleTriggerImpl)?.RepeatInterval.TotalSeconds;
            entity.RequestUrl = jobDetail.JobDataMap.GetString(Constant.RequestUrl);
            entity.BeginTime = triggers.StartTimeUtc.LocalDateTime;
            entity.EndTime = triggers.EndTimeUtc?.LocalDateTime;
            entity.IntervalSecond = intervalSeconds.HasValue ? Convert.ToInt32(intervalSeconds.Value) : 0;
            entity.JobGroup = jobGroup;
            entity.JobName = jobName;
            entity.Cron = (triggers as CronTriggerImpl)?.CronExpressionString;
            entity.RunTimes = (triggers as SimpleTriggerImpl)?.RepeatCount;
            entity.TriggerType = triggers is SimpleTriggerImpl ? TriggerTypeEnum.Simple : TriggerTypeEnum.Cron;
            entity.RequestType = (HttpMethod)int.Parse(jobDetail.JobDataMap.GetString(Constant.RequestType));
            entity.RequestParameters = jobDetail.JobDataMap.GetString(Constant.RequestParameters);
            entity.Headers = jobDetail.JobDataMap.GetString(Constant.Headers);
            entity.MailMessage = (MailMessageEnum)int.Parse(jobDetail.JobDataMap.GetString(Constant.MailMessage) ?? "0");
            entity.Description = jobDetail.Description;
            return entity;
        }

        /// <summary>
        /// 移除异常信息
        /// 因为只能在IJob持久化操作JobDataMap，所有这里直接暴力操作数据库。.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RemoveErrLog(string jobGroup, string jobName)
        {
            using (var connection = new SqliteConnection("Data Source=File/sqliteScheduler.db"))
            {
                string sql = $@"SELECT
                                    JOB_DATA
                                FROM
                                    QRTZ_JOB_DETAILS
                                WHERE
                                    JOB_NAME = @jobName
                                AND JOB_GROUP = @jobGroup";

                var byteArray = await connection.ExecuteScalarAsync<byte[]>(sql, new { jobName, jobGroup });
                var jsonStr = Encoding.Default.GetString(byteArray);
                JObject source = JObject.Parse(jsonStr);
                source.Remove("Exception");// 移除异常日志
                var modifySql = $@"UPDATE QRTZ_JOB_DETAILS
                                    SET JOB_DATA = @jobData
                                    WHERE
                                        JOB_NAME = @jobName
                                    AND JOB_GROUP = @jobGroup;";
                await connection.ExecuteAsync(modifySql, new { jobName, jobGroup, jobData = source.ToString() });
            }

            var jobKey = new JobKey(jobName, jobGroup);
            var jobDetail = await this.scheduler.GetJobDetail(jobKey);
            jobDetail.JobDataMap[Constant.Exception] = string.Empty;

            return true;
        }

        /// <summary>
        /// 获取所有Job信息（简要信息 - 刷新数据的时候使用）.
        /// </summary>
        /// <returns></returns>
        private async Task<List<JobBriefInfoEntity>> GetAllJobBriefInfoAsync()
        {
            List<JobKey> jboKeyList = new List<JobKey>();
            List<JobBriefInfoEntity> jobInfoList = new List<JobBriefInfoEntity>();
            var groupNames = await this.scheduler.GetJobGroupNames();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jboKeyList.AddRange(await this.scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
                jobInfoList.Add(new JobBriefInfoEntity() { GroupName = groupName });
            }

            foreach (var jobKey in jboKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await this.scheduler.GetJobDetail(jobKey);
                var triggersList = await this.scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();

                foreach (var jobInfo in jobInfoList)
                {
                    if (jobInfo.GroupName == jobKey.Group)
                    {
                        jobInfo.JobInfoList.Add(new JobBriefInfo()
                        {
                            Name = jobKey.Name,
                            LastErrMsg = jobDetail.JobDataMap.GetString(Constant.Exception),
                            TriggerState = await this.scheduler.GetTriggerState(triggers.Key),
                            PreviousFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime,
                            NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime,
                        });
                        continue;
                    }
                }
            }

            return jobInfoList;
        }

        private async Task<BaseResult> AddScheduleJobAsync(TaskScheduleModel entity)
        {
            var result = new BaseResult();
            try
            {
                if (await this.scheduler.CheckExists(entity.JobKey))
                {
                    result.Code = 500;
                    result.Msg = "任务已存在";
                    return result;
                }

                var map = new Dictionary<string, string>()
                {
                     { nameof(Constant.RequestUrl), entity.Url },
                     { nameof(Constant.RequestParameters), entity.Parameters },
                     { nameof(Constant.RequestType), entity.HttpMethod.ToString() },
                     { Constant.Headers, ""},
                     { Constant.MailMessage, entity.Id.ToString() },
                };

                IJobDetail job = JobBuilder.CreateForAsync<HttpJob>()
                    .SetJobData(new JobDataMap(map))
                    .WithDescription(entity.Description)
                    .WithIdentity(entity.Name, entity.Group)
                    .Build();

                ITrigger trigger = this.CreateTrigger(entity);

                await this.scheduler.ScheduleJob(job, trigger);
                result.Code = 200;
            }
            catch (Exception ex)
            {
                result.Code = 505;
                result.Msg = ex.Message;
            }

            return result;
        }

        private ITrigger CreateTrigger(TaskScheduleModel entity)
        {
            var builder = TriggerBuilder.Create()
                   .WithIdentity(entity.TriggerKey)
                   .StartAt((DateTimeOffset)entity.StartTime)
                   .EndAt(entity.EndTime);

            switch (entity.TriggerType)
            {
                case TriggerTypeEnum.Cron:
                    if (!CronExpression.IsValidExpression(entity.CronExpression))
                    {
                        throw new Exception();
                    }

                    builder = builder.WithCronSchedule(entity.CronExpression);
                    break;
                case TriggerTypeEnum.Simple:
                    Action<SimpleScheduleBuilder> action;
                    if (entity.RunTimes.HasValue && entity.RunTimes > 0)
                    {
                        action = x => x.WithIntervalInSeconds(entity.IntervalTime.Value)
                              .WithRepeatCount(entity.RunTimes.Value);
                    }
                    else
                    {
                        action = x => x.WithIntervalInSeconds(entity.IntervalTime.Value)
                           .RepeatForever();
                    }

                    builder = builder.WithSimpleSchedule(action);
                    break;
                default:
                    throw new ArgumentException();
            }

            return builder.ForJob(entity.JobKey).Build();
        }
    }
}
