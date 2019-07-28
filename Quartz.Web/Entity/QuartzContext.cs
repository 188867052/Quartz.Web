using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Quartz.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Quartz.Entity
{
    public partial class QuartzContext : DbContext
    {
        public QuartzContext()
        {
        }

        public QuartzContext(DbContextOptions<QuartzContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Icon> Icon { get; set; }

        public virtual DbSet<QuartzBlobTriggers> QuartzBlobTriggers { get; set; }

        public virtual DbSet<QuartzCalendars> QuartzCalendars { get; set; }

        public virtual DbSet<QuartzCronTriggers> QuartzCronTriggers { get; set; }

        public virtual DbSet<QuartzFiredTriggers> QuartzFiredTriggers { get; set; }

        public virtual DbSet<QuartzJobDetails> QuartzJobDetails { get; set; }

        public virtual DbSet<QuartzLocks> QuartzLocks { get; set; }

        public virtual DbSet<QuartzLog> QuartzLog { get; set; }

        public virtual DbSet<QuartzPausedTriggerGrps> QuartzPausedTriggerGrps { get; set; }

        public virtual DbSet<QuartzSchedulerState> QuartzSchedulerState { get; set; }

        public virtual DbSet<QuartzSimpleTriggers> QuartzSimpleTriggers { get; set; }

        public virtual DbSet<QuartzSimpropTriggers> QuartzSimpropTriggers { get; set; }

        public virtual DbSet<QuartzTriggers> QuartzTriggers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(AppSettingManager.AppSettings.Connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Icon>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Icon_name_unique")
                    .IsUnique();

                entity.HasIndex(e => e.Sort)
                    .HasName("ix_sort");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Sort).HasColumnName("sort");
            });

            modelBuilder.Entity<QuartzBlobTriggers>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("quartz_blob_triggers");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.TriggerName)
                    .HasColumnName("trigger_name")
                    .HasMaxLength(150);

                entity.Property(e => e.TriggerGroup)
                    .HasColumnName("trigger_group")
                    .HasMaxLength(150);

                entity.Property(e => e.BlobData).HasColumnName("blob_data");

                entity.HasOne(d => d.QuartzTriggers)
                    .WithOne(p => p.QuartzBlobTriggers)
                    .HasForeignKey<QuartzBlobTriggers>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("FK__quartz_blob_trig");
            });

            modelBuilder.Entity<QuartzCalendars>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.CalendarName });

                entity.ToTable("quartz_calendars");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.CalendarName)
                    .HasColumnName("calendar_name")
                    .HasMaxLength(200);

                entity.Property(e => e.Calendar)
                    .IsRequired()
                    .HasColumnName("calendar");
            });

            modelBuilder.Entity<QuartzCronTriggers>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("quartz_cron_triggers");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.TriggerName)
                    .HasColumnName("trigger_name")
                    .HasMaxLength(150);

                entity.Property(e => e.TriggerGroup)
                    .HasColumnName("trigger_group")
                    .HasMaxLength(150);

                entity.Property(e => e.CronExpression)
                    .IsRequired()
                    .HasColumnName("cron_expression")
                    .HasMaxLength(250);

                entity.Property(e => e.TimeZoneId)
                    .HasColumnName("time_zone_id")
                    .HasMaxLength(80);

                entity.HasOne(d => d.QuartzTriggers)
                    .WithOne(p => p.QuartzCronTriggers)
                    .HasForeignKey<QuartzCronTriggers>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("FK__quartz_cron_trig__2B947552");
            });

            modelBuilder.Entity<QuartzFiredTriggers>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.EntryId });

                entity.ToTable("quartz_fired_triggers");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.EntryId)
                    .HasColumnName("entry_id")
                    .HasMaxLength(140);

                entity.Property(e => e.FiredTime).HasColumnName("fired_time");

                entity.Property(e => e.InstanceName)
                    .IsRequired()
                    .HasColumnName("instance_name")
                    .HasMaxLength(200);

                entity.Property(e => e.IsNonconcurrent).HasColumnName("is_nonconcurrent");

                entity.Property(e => e.JobGroup)
                    .HasColumnName("job_group")
                    .HasMaxLength(150);

                entity.Property(e => e.JobName)
                    .HasColumnName("job_name")
                    .HasMaxLength(150);

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.RequestsRecovery).HasColumnName("requests_recovery");

                entity.Property(e => e.SchedTime).HasColumnName("sched_time");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("state")
                    .HasMaxLength(16);

                entity.Property(e => e.TriggerGroup)
                    .IsRequired()
                    .HasColumnName("trigger_group")
                    .HasMaxLength(150);

                entity.Property(e => e.TriggerName)
                    .IsRequired()
                    .HasColumnName("trigger_name")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<QuartzJobDetails>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.JobName, e.JobGroup });

                entity.ToTable("quartz_job_details");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.JobName)
                    .HasColumnName("job_name")
                    .HasMaxLength(150);

                entity.Property(e => e.JobGroup)
                    .HasColumnName("job_group")
                    .HasMaxLength(150);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(250);

                entity.Property(e => e.IsDurable).HasColumnName("is_durable");

                entity.Property(e => e.IsNonconcurrent).HasColumnName("is_nonconcurrent");

                entity.Property(e => e.IsUpdateData).HasColumnName("is_update_data");

                entity.Property(e => e.JobClassName)
                    .IsRequired()
                    .HasColumnName("job_class_name")
                    .HasMaxLength(250);

                entity.Property(e => e.JobData).HasColumnName("job_data");

                entity.Property(e => e.RequestsRecovery).HasColumnName("requests_recovery");
            });

            modelBuilder.Entity<QuartzLocks>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.LockName });

                entity.ToTable("quartz_locks");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.LockName)
                    .HasColumnName("lock_name")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<QuartzLog>(entity =>
            {
                entity.ToTable("quartz_log");

                entity.HasIndex(e => e.CreateTime)
                    .HasName("ix_create_time");

                entity.HasIndex(e => e.Group)
                    .HasName("ix_group");

                entity.HasIndex(e => e.Name)
                    .HasName("ix_name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Group)
                    .HasColumnName("group")
                    .HasMaxLength(50);

                entity.Property(e => e.LogLevel)
                    .HasConversion(new EnumToStringConverter<LogLevel>())
                    .IsRequired()
                    .HasColumnName("log_level")
                    .HasMaxLength(50);

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.TestData)
                    .HasConversion(new EnumToStringConverter<HttpMethod>())
                    .IsRequired()
                    .HasColumnName("test_data")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<QuartzPausedTriggerGrps>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerGroup });

                entity.ToTable("quartz_paused_trigger_grps");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.TriggerGroup)
                    .HasColumnName("trigger_group")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<QuartzSchedulerState>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.InstanceName });

                entity.ToTable("quartz_scheduler_state");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.InstanceName)
                    .HasColumnName("instance_name")
                    .HasMaxLength(200);

                entity.Property(e => e.CheckinInterval).HasColumnName("checkin_interval");

                entity.Property(e => e.LastCheckinTime).HasColumnName("last_checkin_time");
            });

            modelBuilder.Entity<QuartzSimpleTriggers>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("quartz_simple_triggers");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.TriggerName)
                    .HasColumnName("trigger_name")
                    .HasMaxLength(150);

                entity.Property(e => e.TriggerGroup)
                    .HasColumnName("trigger_group")
                    .HasMaxLength(150);

                entity.Property(e => e.RepeatCount).HasColumnName("repeat_count");

                entity.Property(e => e.RepeatInterval).HasColumnName("repeat_interval");

                entity.Property(e => e.TimesTriggered).HasColumnName("times_triggered");

                entity.HasOne(d => d.QuartzTriggers)
                    .WithOne(p => p.QuartzSimpleTriggers)
                    .HasForeignKey<QuartzSimpleTriggers>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("FK__quartz_simple_tr__25DB9BFC");
            });

            modelBuilder.Entity<QuartzSimpropTriggers>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("quartz_simprop_triggers");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.TriggerName)
                    .HasColumnName("trigger_name")
                    .HasMaxLength(150);

                entity.Property(e => e.TriggerGroup)
                    .HasColumnName("trigger_group")
                    .HasMaxLength(150);

                entity.Property(e => e.BoolProp1).HasColumnName("bool_prop_1");

                entity.Property(e => e.BoolProp2).HasColumnName("bool_prop_2");

                entity.Property(e => e.DecProp1)
                    .HasColumnName("dec_prop_1")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.DecProp2)
                    .HasColumnName("dec_prop_2")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.IntProp1).HasColumnName("int_prop_1");

                entity.Property(e => e.IntProp2).HasColumnName("int_prop_2");

                entity.Property(e => e.LongProp1).HasColumnName("long_prop_1");

                entity.Property(e => e.LongProp2).HasColumnName("long_prop_2");

                entity.Property(e => e.StrProp1)
                    .HasColumnName("str_prop_1")
                    .HasMaxLength(512);

                entity.Property(e => e.StrProp2)
                    .HasColumnName("str_prop_2")
                    .HasMaxLength(512);

                entity.Property(e => e.StrProp3)
                    .HasColumnName("str_prop_3")
                    .HasMaxLength(512);

                entity.Property(e => e.TimeZoneId)
                    .HasColumnName("time_zone_id")
                    .HasMaxLength(80);

                entity.HasOne(d => d.QuartzTriggers)
                    .WithOne(p => p.QuartzSimpropTriggers)
                    .HasForeignKey<QuartzSimpropTriggers>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                    .HasConstraintName("FK__quartz_simprop_t__28B808A7");
            });

            modelBuilder.Entity<QuartzTriggers>(entity =>
            {
                entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup });

                entity.ToTable("quartz_triggers");

                entity.Property(e => e.SchedName)
                    .HasColumnName("sched_name")
                    .HasMaxLength(120);

                entity.Property(e => e.TriggerName)
                    .HasColumnName("trigger_name")
                    .HasMaxLength(150);

                entity.Property(e => e.TriggerGroup)
                    .HasColumnName("trigger_group")
                    .HasMaxLength(150);

                entity.Property(e => e.CalendarName)
                    .HasColumnName("calendar_name")
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(250);

                entity.Property(e => e.EndTime)
                    .HasConversion(new DateTimeToTicksConverter())
                    .HasColumnName("end_time");

                entity.Property(e => e.JobData).HasColumnName("job_data");

                entity.Property(e => e.JobGroup)
                    .IsRequired()
                    .HasColumnName("job_group")
                    .HasMaxLength(150);

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnName("job_name")
                    .HasMaxLength(150);

                entity.Property(e => e.MisfireInstr).HasColumnName("misfire_instr");

                entity.Property(e => e.NextFireTime)
                    .HasConversion(new DateTimeToTicksConverter())
                    .HasColumnName("next_fire_time");

                entity.Property(e => e.PrevFireTime)
                    .HasConversion(new DateTimeToTicksConverter())
                    .HasColumnName("prev_fire_time");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.StartTime)
                    .HasConversion(new DateTimeToTicksConverter())
                    .HasColumnName("start_time");

                entity.Property(e => e.TriggerState)
                    .IsRequired()
                    .HasColumnName("trigger_state")
                    .HasMaxLength(16);

                entity.Property(e => e.TriggerType)
                    .IsRequired()
                    .HasColumnName("trigger_type")
                    .HasMaxLength(8);

                entity.HasOne(d => d.QuartzJobDetails)
                    .WithMany(p => p.QuartzTriggers)
                    .HasForeignKey(d => new { d.SchedName, d.JobName, d.JobGroup })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__quartz_triggers__22FF2F51");
            });
        }
    }
}
