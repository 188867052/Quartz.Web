namespace MaterialUI.Dapper
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AppSettingManager;
    using global::Dapper;
    using global::Dapper.Common;
    using MaterialUI.Entity;

    public partial class DapperExtension
    {
        private static IDbConnection Connection => new SqlConnection(AppSettings.Connection);

        private static IDbContext DbContext => DbContextFactory.GetDbContext(nameof(DataSourceType.SqlServer));

        static DapperExtension()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DbContextFactory.AddDataSource(new DataSource()
            {
                SourceType = DataSourceType.SqlServer,
                Source = () => Connection,
                UseProxy = true,
                Name = DataSourceType.SqlServer.ToString(),
            });
        }

        // TODO: Table name may not be id.
        public static T Find<T>(int id)
        {
            using (Connection)
            {
                string tableName = MetaData.Mapping[typeof(T).Name];
                string sql = $"SELECT * FROM {tableName} WHERE id=@id";
                return Connection.QueryFirstOrDefault<T>(sql, new { id });
            }
        }

        public static IEnumerable<T> FindAll<T>()
        {
            using (Connection)
            {
                string tableName = MetaData.Mapping[typeof(T).Name];
                string sql = $"SELECT * FROM {tableName}";
                return Connection.Query<T>(sql);
            }
        }

        public static int Count<T>()
            where T : class
        {
            return Query<T>().Count();
        }

        public static IQueryable<T> Page<T>(int index, int count, out int total)
            where T : class
        {
            return Query<T>().Page(index, count, out total);
        }

        public static IQueryable<T> Query<T>()
        where T : class
        {
            return DbContext.From<T>();
        }

        public static IEnumerable<T> Page<T>(int size)
        {
            using (Connection)
            {
                string tableName = MetaData.Mapping[typeof(T).Name];
                string sql = $"SELECT TOP {size} * FROM {tableName}";
                return Connection.Query<T>(sql);
            }
        }
    }
}