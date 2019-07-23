﻿namespace MaterialUI.Entity
{
    using System.Collections.Generic;

    public partial class MetaData
    {
        public static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "Icon", "Icon" },
            { "Icon.Id", "id" },
            { "Icon.Name", "name" },
            { "Log", "log" },
            { "Log.Id", "id" },
            { "Log.Message", "message" },
            { "Log.CreateTime", "create_time" },
            { "Log.LogLevel", "log_level" },
            { "Log.Type", "type" },
            { "QrtzBlobTriggers", "QRTZ_BLOB_TRIGGERS" },
            { "QrtzBlobTriggers.SchedName", "SCHED_NAME" },
            { "QrtzBlobTriggers.TriggerName", "TRIGGER_NAME" },
            { "QrtzBlobTriggers.TriggerGroup", "TRIGGER_GROUP" },
            { "QrtzBlobTriggers.BlobData", "BLOB_DATA" },
            { "QrtzCalendars", "QRTZ_CALENDARS" },
            { "QrtzCalendars.SchedName", "SCHED_NAME" },
            { "QrtzCalendars.CalendarName", "CALENDAR_NAME" },
            { "QrtzCalendars.Calendar", "CALENDAR" },
            { "QrtzCronTriggers", "QRTZ_CRON_TRIGGERS" },
            { "QrtzCronTriggers.SchedName", "SCHED_NAME" },
            { "QrtzCronTriggers.TriggerName", "TRIGGER_NAME" },
            { "QrtzCronTriggers.TriggerGroup", "TRIGGER_GROUP" },
            { "QrtzCronTriggers.CronExpression", "CRON_EXPRESSION" },
            { "QrtzCronTriggers.TimeZoneId", "TIME_ZONE_ID" },
            { "QrtzFiredTriggers", "QRTZ_FIRED_TRIGGERS" },
            { "QrtzFiredTriggers.SchedName", "SCHED_NAME" },
            { "QrtzFiredTriggers.EntryId", "ENTRY_ID" },
            { "QrtzFiredTriggers.TriggerName", "TRIGGER_NAME" },
            { "QrtzFiredTriggers.TriggerGroup", "TRIGGER_GROUP" },
            { "QrtzFiredTriggers.InstanceName", "INSTANCE_NAME" },
            { "QrtzFiredTriggers.FiredTime", "FIRED_TIME" },
            { "QrtzFiredTriggers.SchedTime", "SCHED_TIME" },
            { "QrtzFiredTriggers.Priority", "PRIORITY" },
            { "QrtzFiredTriggers.State", "STATE" },
            { "QrtzFiredTriggers.JobName", "JOB_NAME" },
            { "QrtzFiredTriggers.JobGroup", "JOB_GROUP" },
            { "QrtzFiredTriggers.IsNonconcurrent", "IS_NONCONCURRENT" },
            { "QrtzFiredTriggers.RequestsRecovery", "REQUESTS_RECOVERY" },
            { "QrtzJobDetails", "QRTZ_JOB_DETAILS" },
            { "QrtzJobDetails.SchedName", "SCHED_NAME" },
            { "QrtzJobDetails.JobName", "JOB_NAME" },
            { "QrtzJobDetails.JobGroup", "JOB_GROUP" },
            { "QrtzJobDetails.Description", "DESCRIPTION" },
            { "QrtzJobDetails.JobClassName", "JOB_CLASS_NAME" },
            { "QrtzJobDetails.IsDurable", "IS_DURABLE" },
            { "QrtzJobDetails.IsNonconcurrent", "IS_NONCONCURRENT" },
            { "QrtzJobDetails.IsUpdateData", "IS_UPDATE_DATA" },
            { "QrtzJobDetails.RequestsRecovery", "REQUESTS_RECOVERY" },
            { "QrtzJobDetails.JobData", "JOB_DATA" },
            { "QrtzLocks", "QRTZ_LOCKS" },
            { "QrtzLocks.SchedName", "SCHED_NAME" },
            { "QrtzLocks.LockName", "LOCK_NAME" },
            { "QrtzPausedTriggerGrps", "QRTZ_PAUSED_TRIGGER_GRPS" },
            { "QrtzPausedTriggerGrps.SchedName", "SCHED_NAME" },
            { "QrtzPausedTriggerGrps.TriggerGroup", "TRIGGER_GROUP" },
            { "QrtzSchedulerState", "QRTZ_SCHEDULER_STATE" },
            { "QrtzSchedulerState.SchedName", "SCHED_NAME" },
            { "QrtzSchedulerState.InstanceName", "INSTANCE_NAME" },
            { "QrtzSchedulerState.LastCheckinTime", "LAST_CHECKIN_TIME" },
            { "QrtzSchedulerState.CheckinInterval", "CHECKIN_INTERVAL" },
            { "QrtzSimpleTriggers", "QRTZ_SIMPLE_TRIGGERS" },
            { "QrtzSimpleTriggers.SchedName", "SCHED_NAME" },
            { "QrtzSimpleTriggers.TriggerName", "TRIGGER_NAME" },
            { "QrtzSimpleTriggers.TriggerGroup", "TRIGGER_GROUP" },
            { "QrtzSimpleTriggers.RepeatCount", "REPEAT_COUNT" },
            { "QrtzSimpleTriggers.RepeatInterval", "REPEAT_INTERVAL" },
            { "QrtzSimpleTriggers.TimesTriggered", "TIMES_TRIGGERED" },
            { "QrtzSimpropTriggers", "QRTZ_SIMPROP_TRIGGERS" },
            { "QrtzSimpropTriggers.SchedName", "SCHED_NAME" },
            { "QrtzSimpropTriggers.TriggerName", "TRIGGER_NAME" },
            { "QrtzSimpropTriggers.TriggerGroup", "TRIGGER_GROUP" },
            { "QrtzSimpropTriggers.StrProp1", "STR_PROP_1" },
            { "QrtzSimpropTriggers.StrProp2", "STR_PROP_2" },
            { "QrtzSimpropTriggers.StrProp3", "STR_PROP_3" },
            { "QrtzSimpropTriggers.IntProp1", "INT_PROP_1" },
            { "QrtzSimpropTriggers.IntProp2", "INT_PROP_2" },
            { "QrtzSimpropTriggers.LongProp1", "LONG_PROP_1" },
            { "QrtzSimpropTriggers.LongProp2", "LONG_PROP_2" },
            { "QrtzSimpropTriggers.DecProp1", "DEC_PROP_1" },
            { "QrtzSimpropTriggers.DecProp2", "DEC_PROP_2" },
            { "QrtzSimpropTriggers.BoolProp1", "BOOL_PROP_1" },
            { "QrtzSimpropTriggers.BoolProp2", "BOOL_PROP_2" },
            { "QrtzSimpropTriggers.TimeZoneId", "TIME_ZONE_ID" },
            { "QrtzTriggers", "QRTZ_TRIGGERS" },
            { "QrtzTriggers.SchedName", "SCHED_NAME" },
            { "QrtzTriggers.TriggerName", "TRIGGER_NAME" },
            { "QrtzTriggers.TriggerGroup", "TRIGGER_GROUP" },
            { "QrtzTriggers.JobName", "JOB_NAME" },
            { "QrtzTriggers.JobGroup", "JOB_GROUP" },
            { "QrtzTriggers.Description", "DESCRIPTION" },
            { "QrtzTriggers.NextFireTime", "NEXT_FIRE_TIME" },
            { "QrtzTriggers.PrevFireTime", "PREV_FIRE_TIME" },
            { "QrtzTriggers.Priority", "PRIORITY" },
            { "QrtzTriggers.TriggerState", "TRIGGER_STATE" },
            { "QrtzTriggers.TriggerType", "TRIGGER_TYPE" },
            { "QrtzTriggers.StartTime", "START_TIME" },
            { "QrtzTriggers.EndTime", "END_TIME" },
            { "QrtzTriggers.CalendarName", "CALENDAR_NAME" },
            { "QrtzTriggers.MisfireInstr", "MISFIRE_INSTR" },
            { "QrtzTriggers.JobData", "JOB_DATA" },
            { "TaskSchedule", "task_schedule" },
            { "TaskSchedule.Id", "id" },
            { "TaskSchedule.Name", "name" },
            { "TaskSchedule.Status", "status" },
            { "TaskSchedule.Url", "url" },
            { "TaskSchedule.ExceptionMessage", "exception_message" },
            { "TaskSchedule.StartTime", "start_time" },
            { "TaskSchedule.EndTime", "end_time" },
            { "TaskSchedule.LastExcuteTime", "last_excute_time" },
            { "TaskSchedule.NextExcuteTime", "next_excute_time" },
            { "TaskSchedule.ExcutePlan", "excute_plan" },
            { "TaskSchedule.HttpMethod", "http_method" },
            { "TaskSchedule.CronExpression", "cron_expression" },
            { "TaskSchedule.IntervalTime", "interval_time" },
            { "TaskSchedule.IntervalType", "interval_type" },
            { "TaskSchedule.IsEnable", "is_enable" },
            { "TaskSchedule.TriggerType", "trigger_type" },
            { "TaskSchedule.Group", "group" },
            { "TaskSchedule.Parameters", "Parameters" },
            { "TaskSchedule.Description", "Description" },
            { "TaskSchedule.RunTimes", "RunTimes" },
            { "TaskSchedule.IconClass", "icon_class" },
            { "TriggerType", "trigger_type" },
            { "TriggerType.Id", "id" },
            { "TriggerType.Name", "name" },
        };
    }
}
