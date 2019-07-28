namespace Quartz.Controllers
{
    using System.Collections.Generic;

    internal static class ScaffoldingHelper
    {
        internal static IList<string> Scaffolding(string folder)
        {
            DbContextGenerator generator = new DbContextGenerator(folder);

            generator.WriteTo();
            IList<string> changedFiles = generator.EntityCodeList;
            changedFiles.Add(generator.DbContextCode);

            return changedFiles;
        }
    }
}
