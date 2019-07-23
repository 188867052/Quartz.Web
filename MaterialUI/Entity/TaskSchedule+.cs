namespace MaterialKit.Entity
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Quartz;

    public partial class TaskSchedule
    {
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
