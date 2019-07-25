drop table if exists quartz_fired_triggers;
drop table if exists quartz_paused_trigger_grps;
drop table if exists quartz_scheduler_state;
drop table if exists quartz_locks;
drop table if exists quartz_simprop_triggers;
drop table if exists quartz_simple_triggers;
drop table if exists quartz_cron_triggers;
drop table if exists quartz_blob_triggers;
drop table if exists quartz_triggers;
drop table if exists quartz_job_details;
drop table if exists quartz_calendars;
create table quartz_job_details
  (
    sched_name nvarchar(120) not null,
	job_name nvarchar(150) not null,
    job_group nvarchar(150) not null,
    description nvarchar(250) null,
    job_class_name   nvarchar(250) not null,
    is_durable bit not null,
    is_nonconcurrent bit not null,
    is_update_data bit  not null,
	requests_recovery bit not null,
    job_data varbinary(max) null,
    primary key (sched_name,job_name,job_group)
);

create table quartz_triggers
  (
    sched_name nvarchar(120) not null,
	trigger_name nvarchar(150) not null,
    trigger_group nvarchar(150) not null,
    job_name nvarchar(150) not null,
    job_group nvarchar(150) not null,
    description nvarchar(250) null,
    next_fire_time bigint null,
    prev_fire_time bigint null,
    priority integer null,
    trigger_state nvarchar(16) not null,
    trigger_type nvarchar(8) not null,
    start_time bigint not null,
    end_time bigint null,
    calendar_name nvarchar(200) null,
    misfire_instr integer null,
    job_data varbinary(max) null,
    primary key (sched_name,trigger_name,trigger_group),
    foreign key (sched_name,job_name,job_group)
        references quartz_job_details(sched_name,job_name,job_group)
);

create table quartz_simple_triggers
  (
    sched_name nvarchar(120) not null,
	trigger_name nvarchar(150) not null,
    trigger_group nvarchar(150) not null,
    repeat_count bigint not null,
    repeat_interval bigint not null,
    times_triggered bigint not null,
    primary key (sched_name,trigger_name,trigger_group),
    foreign key (sched_name,trigger_name,trigger_group)
        references quartz_triggers(sched_name,trigger_name,trigger_group) on delete cascade
);



create table quartz_simprop_triggers 
  (
    sched_name nvarchar (120) not null ,
    trigger_name nvarchar (150) not null ,
    trigger_group nvarchar (150) not null ,
    str_prop_1 nvarchar (512) null,
    str_prop_2 nvarchar (512) null,
    str_prop_3 nvarchar (512) null,
    int_prop_1 int null,
    int_prop_2 int null,
    long_prop_1 bigint null,
    long_prop_2 bigint null,
    dec_prop_1 numeric null,
    dec_prop_2 numeric null,
    bool_prop_1 bit null,
    bool_prop_2 bit null,
    time_zone_id nvarchar(80) null,
	primary key (sched_name,trigger_name,trigger_group),
	foreign key (sched_name,trigger_name,trigger_group)
        references quartz_triggers(sched_name,trigger_name,trigger_group) on delete cascade
);



create table quartz_cron_triggers
  (
    sched_name nvarchar(120) not null,
	trigger_name nvarchar(150) not null,
    trigger_group nvarchar(150) not null,
    cron_expression nvarchar(250) not null,
    time_zone_id nvarchar(80),
    primary key (sched_name,trigger_name,trigger_group),
    foreign key (sched_name,trigger_name,trigger_group)
        references quartz_triggers(sched_name,trigger_name,trigger_group) on delete cascade
);


create table quartz_blob_triggers
  (
    sched_name nvarchar(120) not null,
	trigger_name nvarchar(150) not null,
    trigger_group nvarchar(150) not null,
    blob_data varbinary(max) null,
    primary key (sched_name,trigger_name,trigger_group),
    foreign key (sched_name,trigger_name,trigger_group)
        references quartz_triggers(sched_name,trigger_name,trigger_group) on delete cascade
);


create table quartz_calendars
  (
    sched_name nvarchar(120) not null,
	calendar_name  nvarchar(200) not null,
    calendar varbinary(max) not null,
    primary key (sched_name,calendar_name)
);

create table quartz_paused_trigger_grps
  (
    sched_name nvarchar(120) not null,
	trigger_group nvarchar(150) not null, 
    primary key (sched_name,trigger_group)
);

create table quartz_fired_triggers
  (
    sched_name nvarchar(120) not null,
	entry_id nvarchar(140) not null,
    trigger_name nvarchar(150) not null,
    trigger_group nvarchar(150) not null,
    instance_name nvarchar(200) not null,
    fired_time bigint not null,
    sched_time bigint not null,
	priority integer not null,
    state nvarchar(16) not null,
    job_name nvarchar(150) null,
    job_group nvarchar(150) null,
    is_nonconcurrent bit null,
    requests_recovery bit null,
    primary key (sched_name,entry_id)
);

create table quartz_scheduler_state
  (
    sched_name nvarchar(120) not null,
	instance_name nvarchar(200) not null,
    last_checkin_time bigint not null,
    checkin_interval bigint not null,
    primary key (sched_name,instance_name)
);

create table quartz_locks
  (
    sched_name nvarchar(120) not null,
	lock_name  nvarchar(40) not null, 
    primary key (sched_name,lock_name)
);
