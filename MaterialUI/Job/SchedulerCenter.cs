namespace MaterialKit.Job
{
    using AppSettingManager;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Impl.AdoJobStore;
    using Quartz.Impl.AdoJobStore.Common;
    using Quartz.Simpl;
    using Quartz.Util;

    /// <summary>
    /// 调度中心.
    /// </summary>
    public class SchedulerCenter
    {
        /// <summary>
        /// 任务调度对象.
        /// </summary>
        public static readonly SchedulerCenter Instance;

        static SchedulerCenter()
        {
            Instance = new SchedulerCenter();
        }

        private IScheduler scheduler;

        /// <summary>
        /// 返回任务计划（调度器）.
        /// </summary>
        /// <returns></returns>
        public IScheduler Scheduler
        {
            get
            {
                if (this.scheduler == null)
                {
                    DbProvider provider = new DbProvider("SqlServer", AppSettings.Connection);
                    DBConnectionManager.Instance.AddConnectionProvider("default", provider);
                    var serializer = new JsonObjectSerializer();
                    serializer.Initialize();
                    var jobStore = new JobStoreTX
                    {
                        DataSource = "default",
                        TablePrefix = "QRTZ_",
                        InstanceId = "AUTO",

                        DriverDelegateType = typeof(SqlServerDelegate).AssemblyQualifiedName,
                        ObjectSerializer = serializer,
                    };

                    DirectSchedulerFactory.Instance.CreateScheduler("benny" + "Scheduler", "AUTO", new DefaultThreadPool(), jobStore);
                    this.scheduler = SchedulerRepository.Instance.Lookup("benny" + "Scheduler").Result;
                    this.scheduler.Start(); // 默认开始调度器
                    return this.scheduler;
                }

                return this.scheduler;
            }
        }
    }
}
