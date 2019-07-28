namespace Quartz.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    internal static class RenamingHelper
    {
        internal static IList<string> Renaming(string webRootPath)
        {
            string path = webRootPath;
            DirectoryInfo di = new DirectoryInfo(path);

            IList<string> changedFiles = new List<string>();
            var files = di.Parent.GetFiles("*.cs", SearchOption.AllDirectories);

            foreach (var item in files)
            {
                Match match = Regex.Match(item.Name, "[{].+[}]");
                if (match != null && match.Length > 0)
                {
                    int count = match.Value.Split(',').Length;
                    string newName = item.FullName.Replace(match.Value, new string('\'', count));

                    File.Move(item.FullName, newName);
                    changedFiles.Add(item.FullName);
                }
            }

            return changedFiles;
        }
    }
}
