﻿namespace Quartz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using AspNetCore.Extensions;
    using Newtonsoft.Json;

    internal static class RouteHelper
    {
        internal static async Task<string> Generate(string baseAddress)
        {
            using (var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
            })
            {
                using (HttpResponseMessage response = await client.GetAsync("routes"))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    return GenerateRoutes(content);
                }
            }
        }

        private static string GenerateRoutes(string content)
        {
            IEnumerable<RouteInfo> infos = JsonConvert.DeserializeObject<IEnumerable<RouteInfo>>(content);
            StringBuilder sb = new StringBuilder();
            var group = infos.GroupBy(o => o.Namespace);
            for (int i = 0; i < group.Count(); i++)
            {
                sb.Append(GenerateNamespace(group.ElementAt(i), i == group.Count() - 1));
            }

            return sb.ToString();
        }

        private static StringBuilder GenerateNamespace(IGrouping<string, RouteInfo> namespaceGroup, bool isLast)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"namespace {GetConvertedNamespace(namespaceGroup.Key)}");
            sb.AppendLine("{");

            var group = namespaceGroup.GroupBy(o => o.ControllerName);
            for (int i = 0; i < group.Count(); i++)
            {
                sb.Append(GenerateClass(group.ElementAt(i), i == group.Count() - 1));
            }

            sb.AppendLine("}");
            if (!isLast)
            {
                sb.AppendLine();
            }

            return sb;
        }

        private static StringBuilder GenerateClass(IGrouping<string, RouteInfo> group, bool isLast)
        {
            string classFullName = $"{group.First().Namespace}.{group.First().ControllerName}Controller";
            string crefNamespace = GetCrefNamespace(classFullName, GetConvertedNamespace(group.First().Namespace));
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"    /// <summary>");
            sb.AppendLine($"    /// <see cref=\"{crefNamespace}\"/>");
            sb.AppendLine($"    /// </summary>");
            sb.AppendLine($"    public class {group.Key}Route");
            sb.AppendLine("    {");
            for (int i = 0; i < group.Count(); i++)
            {
                var item = group.ElementAt(i);
                var renamedAction = RenameOverloadedAction(group, i);
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// <see cref=\"{crefNamespace}.{item.ActionName}\"/>");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public const string {renamedAction} = \"{item.Path}\";");

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

        private static string GetConvertedNamespace(string name)
        {
            return name.Replace("Controllers", "Routes");
        }

        private static string RenameOverloadedAction(IGrouping<string, RouteInfo> group, int index)
        {
            var currentItem = group.ElementAt(index);
            var sameActionNameGroup = group.GroupBy(o => o.ActionName);
            foreach (var item in sameActionNameGroup)
            {
                if (item.Count() > 1)
                {
                    for (int i = 1; i < item.Count(); i++)
                    {
                        var element = item.ElementAt(i);
                        if (element == currentItem)
                        {
                            return element.ActionName + i;
                        }
                    }
                }
            }

            return currentItem.ActionName;
        }

        private static string GetCrefNamespace(string cref, string @namespace)
        {
            IList<string> sameString = new List<string>();
            var splitNamespace = @namespace.Split('.');
            var splitCref = cref.Split('.');
            int minLength = Math.Min(splitNamespace.Length, splitCref.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (splitCref[i] == splitNamespace[i])
                {
                    sameString.Add(splitCref[i]);
                }
                else
                {
                    break;
                }
            }

            cref = cref.Substring(string.Join('.', sameString).Length + 1);
            return cref;
        }
    }
}
