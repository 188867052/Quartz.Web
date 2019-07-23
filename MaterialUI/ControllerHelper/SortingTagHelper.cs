namespace MaterialUI.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using MaterialUI.Html.Tags;

    internal static class SortingTagHelper
    {
        internal static IList<string> Sorting(string webRootPath)
        {
            string path = webRootPath;
            DirectoryInfo di = new DirectoryInfo(path);

            IList<string> changedFiles = new List<string>();
            var files = di.Parent.GetFiles("Tag.cs", SearchOption.AllDirectories);
            foreach (var item in files)
            {
                string precontent = File.ReadAllText(item.FullName);
                var prop = typeof(Tag).GetFields().OrderBy(o => o.Name);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"namespace {typeof(Tag).Namespace}");
                sb.AppendLine("{");
                sb.AppendLine("    [System.Diagnostics.CodeAnalysis.SuppressMessage(\"StyleCop.CSharp.NamingRules\", \"SA1303: Const field names should begin with upper -case letter\", Justification = \"<挂起>\")]");
                sb.AppendLine($"    public class {nameof(Tag)}");
                sb.AppendLine("    {");
                foreach (var p in prop)
                {
                    string value = p.GetRawConstantValue().ToString();
                    sb.AppendLine($"        public const string {p.Name} = \"{value}\";");
                }

                sb.AppendLine("    }");
                sb.AppendLine();

                var prop2 = typeof(Attr).GetFields().OrderBy(o => o.Name);
                sb.AppendLine("    [System.Diagnostics.CodeAnalysis.SuppressMessage(\"StyleCop.CSharp.MaintainabilityRules\", \"SA1402:File may only contain a single type\", Justification = \"<挂起>\")]");
                sb.AppendLine($"    public class {nameof(Attr)}");
                sb.AppendLine("    {");
                foreach (var p in prop2)
                {
                    string value = p.GetRawConstantValue().ToString();
                    sb.AppendLine($"        public const string {p.Name[0].ToString().ToUpper() + p.Name.Substring(1)} = \"{value}\";");
                }

                sb.AppendLine("    }");

                sb.AppendLine("}");

                string postcontent = sb.ToString();
                if (precontent != postcontent)
                {
                    changedFiles.Add(item.FullName);
                }

                File.WriteAllTextAsync(item.FullName, postcontent);
            }

            return changedFiles;
        }
    }
}
