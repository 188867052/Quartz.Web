namespace MaterialUI.Dapper
{
    using System.Collections.Generic;
    using global::Dapper;
    using MaterialUI.Entity;

    public partial class DapperExtension
    {
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
    }
}