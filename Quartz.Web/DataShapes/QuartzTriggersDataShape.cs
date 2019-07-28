namespace Quartz.DataShapes
{
    using Microsoft.EntityFrameworkCore;
    using Quartz.Entity;

    public static class QuartzTriggersDataShape
    {
        public static DbContext Index(DbContext dbContext)
        {
            dbContext.Set<QuartzJobDetails>().Load();
            dbContext.Set<QuartzBlobTriggers>().Load();
            dbContext.Set<QuartzCronTriggers>().Load();
            dbContext.Set<QuartzSimpleTriggers>().Load();
            dbContext.Set<QuartzSimpropTriggers>().Load();
            return dbContext;
        }
    }
}
