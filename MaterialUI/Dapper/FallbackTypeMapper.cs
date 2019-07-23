namespace MaterialKit.Dapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Dapper;

    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> mappers;

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            this.mappers = mappers;
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in this.mappers)
            {
                ConstructorInfo result = mapper.FindConstructor(names, types);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public ConstructorInfo FindExplicitConstructor() => this.mappers
                .Select(mapper => mapper.FindExplicitConstructor())
                .FirstOrDefault(result => result != null);

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in this.mappers)
            {
                var result = mapper.GetConstructorParameter(constructor, columnName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in this.mappers)
            {
                string propertyName = DapperExtension.ToProperty(columnName);
                var result = mapper.GetMember(propertyName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}