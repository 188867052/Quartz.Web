namespace MaterialUI.DataShapes
{
    using MaterialUI.Entity;
    using Microsoft.EntityFrameworkCore;

    public static class QuartzTriggersDataShape
    {
        public static void Index(DbContext dbContext)
        {
            dbContext.Set<QuartzJobDetails>().Load();
            dbContext.Set<QuartzBlobTriggers>().Load();
            dbContext.Set<QuartzCronTriggers>().Load();
            dbContext.Set<QuartzSimpleTriggers>().Load();
            dbContext.Set<QuartzSimpropTriggers>().Load();
        }
    }
}
