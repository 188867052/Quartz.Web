namespace MaterialUI.Entity
{
    using System.Collections.Generic;

    public partial class MetaData
    {
        public static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "Icon", "Icon" },
            { "Log", "log" },
            { "QrtzBlobTriggers", "QRTZ_BLOB_TRIGGERS" },
            { "QrtzCalendars", "QRTZ_CALENDARS" },
            { "QrtzCronTriggers", "QRTZ_CRON_TRIGGERS" },
            { "QrtzFiredTriggers", "QRTZ_FIRED_TRIGGERS" },
            { "QrtzJobDetails", "QRTZ_JOB_DETAILS" },
            { "QrtzLocks", "QRTZ_LOCKS" },
            { "QrtzPausedTriggerGrps", "QRTZ_PAUSED_TRIGGER_GRPS" },
            { "QrtzSchedulerState", "QRTZ_SCHEDULER_STATE" },
            { "QrtzSimpleTriggers", "QRTZ_SIMPLE_TRIGGERS" },
            { "QrtzSimpropTriggers", "QRTZ_SIMPROP_TRIGGERS" },
            { "QrtzTriggers", "QRTZ_TRIGGERS" },
            { "TaskSchedule", "task_schedule" },
            { "TriggerType", "trigger_type" },
        };
    }
}
