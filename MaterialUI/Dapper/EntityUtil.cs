namespace Dapper.Common.Util
{
    using System;
    using System.Collections.Generic;
    using MaterialUI.Entity;

    public class EntityUtil
    {
        private static readonly List<Table> Database = new List<Table>();

        private static Table Build(Type type)
        {
            string key = MetaData.Mapping[$"{type.Name}.PrimaryKey"];
            if (!Database.Exists(e => e.CSharpType == type))
            {
                var properties = type.GetProperties();
                var columns = new List<Column>();
                foreach (var item in properties)
                {
                    // TODO: the dapper issus.
                    if (MetaData.Mapping.TryGetValue($"{type.Name}.{item.Name}", out string value))
                    {
                        columns.Add(new Column()
                        {
                            ColumnKey = ColumnKey.None,
                            ColumnName = value,
                            CSharpName = item.Name,
                            Identity = key == item.Name,
                        });
                    }
                }

                if (columns.Count > 0 && !columns.Exists(e => e.ColumnKey == ColumnKey.Primary))
                {
                    columns[0].ColumnKey = ColumnKey.Primary;
                }

                lock (Database)
                {
                    if (!Database.Exists(e => e.CSharpType == type))
                    {
                        Database.Add(new Table()
                        {
                            CSharpName = type.Name,
                            CSharpType = type,
                            TableName = MetaData.Mapping[type.Name],
                            Columns = columns,
                        });
                    }
                }
            }

            return Database.Find(f => f.CSharpType == type);
        }

        public static Table GetTable<T>()
            where T : class
        {
            return Build(typeof(T));
        }

        public static Table GetTable(Type type)
        {
            return Build(type);
        }

        public static Column GetColumn(Type type, Func<Column, bool> func)
        {
            return Build(type).Columns.Find(f => func(f));
        }
    }

    public class Table
    {
        public Type CSharpType { get; set; }

        public string TableName { get; set; }

        public string CSharpName { get; set; }

        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        public string ColumnName { get; set; }

        public string CSharpName { get; set; }

        public bool Identity { get; set; }

        public ColumnKey ColumnKey { get; set; }
    }
}
