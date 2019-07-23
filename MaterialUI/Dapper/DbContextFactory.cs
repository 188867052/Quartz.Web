namespace Dapper.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public class DbContextFactory
    {
        private static List<DataSource> dataSource = new List<DataSource>();

        static DbContextFactory()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public static DataSource GetDataSource(string name = null)
        {
            if (name == null)
            {
                return dataSource.Find(f => f.Default) ?? dataSource.FirstOrDefault();
            }
            else
            {
                return dataSource.Find(f => f.Name == name);
            }
        }

        public static void AddDataSource(DataSource dataSource)
        {
            DbContextFactory.dataSource.Add(dataSource);

            if (dataSource.Default)
            {
                foreach (var item in DbContextFactory.dataSource)
                {
                    item.Default = false;
                }
            }
        }

        public static IDbContext GetDbContext(string name = null)
        {
            var datasource = GetDataSource(name);
            IDbContext session = null;

            if (datasource.UseProxy)
            {
                session = new DbContextProxy(new DbContext(datasource.Source(), datasource.SourceType));
            }
            else
            {
                session = new DbContext(datasource.Source(), datasource.SourceType);
            }

            return session;
        }
    }
}
