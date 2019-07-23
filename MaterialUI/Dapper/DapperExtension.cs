namespace MaterialKit.Dapper
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using AppSettingManager;
    using global::Dapper;
    using MaterialKit.Entity;
    using Microsoft.EntityFrameworkCore;

    public partial class DapperExtension
    {
        private static IDbConnection Connection => new SqlConnection(AppSettings.Connection);

        static DapperExtension()
        {
            var properties = typeof(MaterialKitContext).GetProperties();
            foreach (var property in properties)
            {
                if (property.ToString().Contains(typeof(DbSet<>).FullName))
                {
                    var type = property.PropertyType.GenericTypeArguments[0];
                    SqlMapper.SetTypeMap(type, new ColumnAttributeTypeMapper(type));
                }
            }
        }

        // TODO: may not work when rule changed.
        internal static string ToProperty(string field)
        {
            var array = field.Split('_');
            return array.Aggregate<string, string>(default, (current, item) => current + char.ToUpper(item[0]) + item.Substring(1));
        }
    }
}