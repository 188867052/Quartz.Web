namespace Dapper.Common
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class FunctionAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class ParameterAttribute : Attribute
    {
    }

    public enum ColumnKey
    {
        None,
        Primary,
        Quniue,
        Unique,
        Foreign,
    }
}
