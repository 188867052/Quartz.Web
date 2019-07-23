﻿namespace MaterialKit.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;

    internal static class TagAnalyzeHelper
    {
        internal static IList<string> Analyzing(string webRootPath)
        {
            string path = webRootPath;
            DirectoryInfo di = new DirectoryInfo(path);

            IList<string> changedFiles = new List<string>();

            var files = di.Parent.GetFiles("*.cs", SearchOption.AllDirectories);
            foreach (var item in files)
            {
                string precontent = File.ReadAllText(item.FullName);
                string postcontent = precontent;
                var prop = typeof(Tag).GetFields();
                foreach (var p in prop)
                {
                    string value = p.GetRawConstantValue().ToString();
                    string pre = $"{nameof(TagHelper)}.{nameof(TagHelper.Create)}(\"{value}\",";
                    string post = $"{nameof(TagHelper)}.{nameof(TagHelper.Create)}({nameof(Tag)}.{p.Name},";
                    postcontent = postcontent.Replace(pre, post);
                }

                var prop2 = typeof(Attr).GetFields();
                var regex = Regex.Match(precontent, @"new TagAttributeList([\s\S]*)");
                if (regex != null && !string.IsNullOrEmpty(regex.Value))
                {
                    string content = regex.Value.Split(';')[0];
                    foreach (var p in prop2)
                    {
                        string value = p.GetRawConstantValue().ToString();
                        string pre = $"{{ \"{value}\", ";
                        string post = $"{{ {nameof(Attr)}.{p.Name}, ";
                        content = content.Replace(pre, post);
                    }

                    postcontent = postcontent.Replace(regex.Value.Split(';')[0], content);
                }

                foreach (var p in prop2)
                {
                    string value = p.GetRawConstantValue().ToString();
                    string pre = $"new {nameof(TagAttribute)}(\"{value}\"";
                    string post = $"new {nameof(TagAttribute)}({nameof(Attr)}.{p.Name}";
                    postcontent = postcontent.Replace(pre, post);
                }

                if (precontent != postcontent)
                {
                    changedFiles.Add(item.FullName);
                    File.WriteAllTextAsync(item.FullName, postcontent);
                }
            }

            return changedFiles;
        }
    }
}
