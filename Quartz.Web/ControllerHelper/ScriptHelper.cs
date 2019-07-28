namespace AspNetCore.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Quartz.Shared;

    internal static class ScriptHelper
    {
        private static string webRootPath;

        internal static string GenerateScript(string wwwroot)
        {
            Check.NotEmpty(wwwroot, nameof(wwwroot));

            webRootPath = wwwroot;
            var directoryInfo = new DirectoryInfo(wwwroot);
            var files = directoryInfo.GetFiles("*", SearchOption.AllDirectories).
                Where(o => (new string[] { ".js", ".css", ".html" }).Contains(o.Extension));

            StringBuilder sb = new StringBuilder();
            var group = files.GroupBy(o => o.Directory.Parent.FullName);
            for (int i = 0; i < group.Count(); i++)
            {
                sb.Append(GenerateNamespace(group.ElementAt(i), i == group.Count() - 1));
            }

            return sb.ToString();
        }

        private static StringBuilder GenerateNamespace(IGrouping<string, FileInfo> namespaceGroup, bool isLast)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"namespace {GetConvertedFileNamespace(namespaceGroup.Key)}");
            sb.AppendLine("{");

            var group = namespaceGroup.GroupBy(o => o.DirectoryName);
            for (int i = 0; i < group.Count(); i++)
            {
                sb.Append(GenerateClass2(group.ElementAt(i), i == group.Count() - 1));
            }

            sb.AppendLine("}");
            if (!isLast)
            {
                sb.AppendLine();
            }

            return sb;
        }

        private static string GetConvertedFileNamespace(string name)
        {
            string relativeName = GetRelativeName(name);
            IEnumerable<string> list0 = new List<string>() { nameof(Quartz), "Files" };
            IList<string> list = relativeName.Split('.', '-', '/').Where(o => !string.IsNullOrEmpty(o)).ToList();
            list0 = list0.Concat(list);
            return string.Join(".", list0.Select(o => char.ToUpper(o[0]) + o.Substring(1)));
        }

        private static string GetFiledName(FileInfo index)
        {
            List<string> list = index.Name.Split('.', '-', '/').Where(o => !string.IsNullOrEmpty(o)).ToList();
            return string.Join("", list.Select(o => char.ToUpper(o[0]) + o.Substring(1)));
        }

        private static StringBuilder GenerateClass2(IGrouping<string, FileInfo> group, bool isLast)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"    public class {GetClassName(group.Key)}");
            sb.AppendLine("    {");
            for (int i = 0; i < group.Count(); i++)
            {
                var item = group.ElementAt(i);
                var renamedAction = GetFiledName(group.ElementAt(i));
                sb.AppendLine($"        public const string {renamedAction} = \"{GetRelativeName(item.FullName)}\";");

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

        private static string GetRelativeName(string FullName)
        {
            return FullName.Replace(webRootPath, "").Replace(@"\", "/");
        }

        internal static string GetClassName(string key)
        {
            string relativeName = GetRelativeName(key);
            var array = relativeName.Split('.', '-', '/').Where(o => !string.IsNullOrEmpty(o)).ToList();
            return string.Join("", array.Select(o => char.ToUpper(o[0]) + o.Substring(1))) + "File";
        }
    }
}
