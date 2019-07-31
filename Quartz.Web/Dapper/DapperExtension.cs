namespace Quartz.Dapper
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AppSettingManager;
    using global::Dapper;
    using Quartz.Entity;

    public partial class DapperExtension
    {
        public static IDbConnection Connection => new SqlConnection(AppSettings.Connection);

        static DapperExtension()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SqlMapper.AddTypeHandler(new DateTimeToTicksHandler());
        }

        // TODO: not support multiple key.
        public static T Find<T>(object id)
        {
            using (Connection)
            {
                string tableName = MetaData.Mapping[typeof(T).Name];
                var pk = MetaData.Mapping[$"{typeof(T).Name}.PrimaryKey"];
                string sql = $"SELECT * FROM {tableName} WHERE {pk}=@{pk}";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(pk, id);
                return Connection.QueryFirstOrDefault<T>(sql, parameters);
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

        public static IEnumerable<T> Page<T>(int size)
        {
            using (Connection)
            {
                string tableName = MetaData.Mapping[typeof(T).Name];
                string sql = $"SELECT TOP {size} * FROM {tableName}";
                return Connection.Query<T>(sql);
            }
        }

        internal static T Query<T>(string sql, object param = null)
        {
            string tableName = MetaData.Mapping[typeof(T).Name];
            sql = $"SELECT * FROM {tableName} WHERE " + sql;
            return Connection.QueryFirst<T>(sql, param);
        }
    }
}