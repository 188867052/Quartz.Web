namespace MaterialUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Extension;
    using Dapper;
    using global::Dapper;
    using MaterialUI.Entity;
    using MaterialUI.Enums;
    using MaterialUI.Framework;
    using MaterialUI.GridConfigurations.Schedule;
    using MaterialUI.Job;
    using MaterialUI.Job.Common;
    using MaterialUI.Job.Entity;
    using MaterialUI.Models;
    using MaterialUI.SearchFilterConfigurations;
    using MaterialUI.ViewConfiguration.Schedule;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using Quartz;
    using Quartz.Impl.Matchers;
    using Quartz.Impl.Triggers;

    public class ScheduleController : StandardController
    {
        public ScheduleController(MaterialKitContext dbContext)
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
            var list = DapperExtension.FindAll<TaskSchedule>().ToList();
            list = this.GetAllJobAsync(list).Result;
            var page = new SchedualGridSearch<TaskSchedule, SchedulePostModel>(list);
            return this.SearchGrid(page);
        }

        public IActionResult DeleteDialog(int id)
        {
            var dialog = new DeleteConfiguration(id);
            return this.Dialog(dialog);
        }

        public IActionResult Edit(int id)
        {
            var entity = DapperExtension.Find<TaskSchedule>(id);
            var dialog = new EditScheduleDialog<TaskSchedule, TaskSchedule>(entity);
            return this.Dialog(dialog);
        }

        public async Task<IActionResult> ReplaceColumn(TriggerTypeEnum type, int? id)
        {
            var entity = new TaskSchedule();
            if (id.HasValue)
            {
                entity = DapperExtension.Find<TaskSchedule>(id.Value);
            }

            entity.TriggerType = type;
            ReplaceLargeColumn<TaskSchedule, TaskSchedule> replaceColumn = new ReplaceLargeColumn<TaskSchedule, TaskSchedule>(entity);
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
        public async Task StopJob(int id)
        {
            var entity = this.DbContext.TaskSchedule.Find(id);
            entity.IsEnable = false;
            entity.IconClass = "pause";
            this.DbContext.SaveChanges();

            await this.scheduler.PauseJob(entity.JobKey);
        }

        /// <summary>
        /// 恢复运行暂停的任务.
        /// </summary>
        public async Task ResumeJob(int id)
        {
            var entity = this.DbContext.TaskSchedule.Find(id);
            entity.IsEnable = true;
            entity.IconClass = "play_circle_filled";
            var jobKey = entity.JobKey;
            this.DbContext.SaveChanges();

            if (await this.scheduler.CheckExists(jobKey))
            {
                await this.scheduler.ResumeJob(jobKey);
            }
        }

        public IActionResult Add()
        {
            var dialog = new EditScheduleDialog<TaskSchedule, TaskSchedule>();
            return this.Dialog(dialog);
        }

        [HttpPost]
        public async Task<IActionResult> Search(SchedulePostModel model)
        {
            IQueryable<TaskSchedule> query = this.DbContext.TaskSchedule.AsNoTracking();
            query = query.AddStringContainsFilter(o => o.Name, model.Name);
            query = query.AddStringContainsFilter(o => o.Group, model.Group);
            query = query.AddStringContainsFilter(o => o.Url, model.Url);
            query = query.AddStringContainsFilter(o => o.CronExpression, model.Cron);
            query = query.AddDateTimeBetweenFilter(model.StartTime, model.EndTime, o => o.StartTime);
            query = query.AddDateTimeBetweenFilter(model.StartTime, model.EndTime, o => o.EndTime);
            var list = query.ToList();

            list = this.GetAllJobAsync(list).Result;
            ScheduleGridConfiguration<TaskSchedule> grid = new ScheduleGridConfiguration<TaskSchedule>(list);

            return this.HtmlResult(grid.Render());
        }

        [HttpPost]
        public async Task<BaseResult> ModifyJob(TaskSchedule model)
        {
            await this.scheduler.PauseJob(model.JobKey);
            await this.scheduler.DeleteJob(model.JobKey);
            await this.AddScheduleJobAsync(model);

            if (model.IsEnable)
            {
                model.IconClass = "play_circle_filled";
            }
            else
            {
                model.IconClass = "pause";
                await this.scheduler.PauseJob(model.JobKey);
            }

            this.DbContext.Update(model);
            this.DbContext.SaveChanges();
            return new BaseResult() { Msg = "修改计划任务成功！" };
        }

        /// <summary>
        /// 获取job日志.
        /// </summary>
        public async Task<IActionResult> GetJobLogs(int id)
        {
            var entity = this.DbContext.TaskSchedule.Find(id);

            IJobDetail jobDetail = await this.scheduler.GetJobDetail(entity.JobKey);
            List<string> list = jobDetail.JobDataMap[Constant.LogList] as List<string>;
            LogDialog dialog = new LogDialog(list);
            return this.HtmlResult(dialog.Render());
        }

        /// <summary>
        /// 启动调度.
        /// </summary>
        [HttpGet]
        public async Task<bool> StartSchedule()
        {
            if (this.scheduler.InStandbyMode)
            {
                await this.scheduler.Start();
                Serilog.Log.Information("任务调度启动！");
            }

            return this.scheduler.InStandbyMode;
        }

        /// <summary>
        /// 停止调度.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> StopSchedule()
        {
            if (!this.scheduler.InStandbyMode)
            {
                // TODO  注意：Shutdown后Start会报错，所以这里使用暂停。
                await this.scheduler.Standby();
                Serilog.Log.Information("任务调度暂停！");
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
        public async Task<BaseResult> AddJob(TaskSchedule entity)
        {
            return await this.AddScheduleJobAsync(entity);
        }

        public JsonResult Delete(int id)
        {
            var entity = this.DbContext.TaskSchedule.Find(id);
            this.DbContext.TaskSchedule.Remove(entity);
            var number = this.DbContext.SaveChanges();
            return this.Json(number);
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

        private async Task<BaseResult> AddScheduleJobAsync(TaskSchedule entity)
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
                     { "RequestUrl", entity.Url },
                     { "RequestParameters", entity.Parameters },
                     { "RequestType", entity.HttpMethod.ToString() },
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

        private ITrigger CreateTrigger(TaskSchedule entity)
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
