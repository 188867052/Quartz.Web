namespace Quartz.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Quartz.Entity;

    internal static class EnumHelper
    {
        private static IList<EnumInfo> info = new List<EnumInfo>();

        internal static string Generate()
        {
            StringBuilder sb = new StringBuilder();
            var group = info.GroupBy(o => o.EnumName);
            sb.AppendLine($"namespace {nameof(Quartz)}.Enums");
            sb.AppendLine("{");
            for (int i = 0; i < group.Count(); i++)
            {
                sb.Append(GenerateEnum(group.ElementAt(i), i == group.Count() - 1));
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static StringBuilder GenerateEnum(IGrouping<string, EnumInfo> group, bool isLast)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"    public enum {group.Key}Enum");
            sb.AppendLine("    {");
            for (int i = 0; i < group.Count(); i++)
            {
                var item = group.ElementAt(i);
                var name = GetFiledName(group.ElementAt(i));
                sb.AppendLine($"        {name} = {item.Id},");

                if (i != group.Count() - 1)
                {
                    sb.AppendLine();
                }
            }

            sb.AppendLine("    }");
            if (!isLast)
            {
                sb.AppendLine();
            }

            return sb;
        }

        private static string GetFiledName(EnumInfo index)
        {
            List<string> list = index.PropertyName.Split('.', '-', '/').Where(o => !string.IsNullOrEmpty(o)).ToList();
            return string.Join("", list.Select(o => char.ToUpper(o[0]) + o.Substring(1)));
        }

        private class EnumInfo
        {
            public string EnumName { get; set; }

            public string PropertyName { get; set; }

            public int Id { get; set; }
        }
    }
}
