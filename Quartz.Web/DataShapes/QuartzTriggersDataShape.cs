namespace Quartz.DataShapes
{
    using Microsoft.EntityFrameworkCore;
    using Quartz.Entity;
    using Quartz.Shared;

    public static class QuartzTriggersDataShape
    {
        public static DbContext Index(DbContext dbContext)
        {
            Check.NotNull(dbContext, nameof(dbContext));

            dbContext.Set<QuartzJobDetails>().Load();
            dbContext.Set<QuartzBlobTriggers>().Load();
            dbContext.Set<QuartzCronTriggers>().Load();
            dbContext.Set<QuartzSimpleTriggers>().Load();
            dbContext.Set<QuartzSimpropTriggers>().Load();
            return dbContext;
        }
    }
}
