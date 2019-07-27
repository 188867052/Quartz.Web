namespace Quartz.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using AspNetCore.Extensions;
    using AspNetCore.Extensions.JsonClassGenerate;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Quartz.Entity;
    using Quartz.Files;
    using Quartz.Framework;
    using Quartz.Html.Tags;
    using Quartz.Routes;

    public class GeneratorController : StandardController
    {
        private readonly IHostingEnvironment env;

        public GeneratorController( IHostingEnvironment hostingEnvironment, QuartzContext materialKitContext)
            : base(materialKitContext)
        {
            this.env = hostingEnvironment;
        }

        [HttpGet]
        [Route(GeneratorRoute.ShowClass)]
        public IActionResult ShowClass()
        {
            var generatedCode = RouteHelper.Generate(AppSettingManager.AppSettings.Instance.BaseAddress).Result;
            string path = Directory.GetFiles(this.env.ContentRootPath, "Routes.Generated.cs", SearchOption.AllDirectories).FirstOrDefault();
            System.IO.File.WriteAllText(path, generatedCode);
            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(generatedCode), "cs");
            return this.HtmlResult(html);
        }

        [HttpGet]
        [Route(GeneratorRoute.GenerateEnum)]
        public IActionResult GenerateEnum()
        {
            var generatedCode = EnumHelper.Generate();
            string path = Directory.GetFiles(this.env.ContentRootPath, "Enums.Generated.cs", SearchOption.AllDirectories).FirstOrDefault();
            System.IO.File.WriteAllText(path, generatedCode);
            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(generatedCode), "cs");
            return this.HtmlResult(html);
        }

        [HttpGet]
        [Route(GeneratorRoute.ShowScript)]
        public IActionResult ShowScript()
        {
            var generatedCode = ScriptHelper.GenerateScript(this.env.WebRootPath);
            string path = Directory.GetFiles(this.env.ContentRootPath, "Files.Generated.cs", SearchOption.AllDirectories).FirstOrDefault();
            System.IO.File.WriteAllText(path, generatedCode);

            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(generatedCode), "cs");
            return this.HtmlResult(html);
        }

        [HttpGet]
        [Route(GeneratorRoute.AppSettings)]
        public IActionResult AppSettings()
        {
            var text = GenerateAppSetting.Generate();
            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(text), "cs");
            return this.HtmlResult(html);
        }

        [HttpGet]
        [Route(GeneratorRoute.SortingTag)]
        public IActionResult SortingTag()
        {
            IList<string> changedFiles = SortingTagHelper.Sorting(this.env.WebRootPath);
            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(JsonConvert.SerializeObject(changedFiles, Formatting.Indented)), "json");
            return this.HtmlResult(html);
        }

        [HttpGet]
        [Route(GeneratorRoute.TagAnalyze)]
        public IActionResult TagAnalyze()
        {
            IList<string> changedFiles = TagAnalyzeHelper.Analyzing(this.env.WebRootPath);
            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(JsonConvert.SerializeObject(changedFiles, Formatting.Indented)), "json");
            return this.HtmlResult(html);
        }

        [HttpGet]
        [Route(GeneratorRoute.RenameFile)]
        public IActionResult RenameFile()
        {
            IList<string> changedFiles = RenamingHelper.Renaming(this.env.WebRootPath);
            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(JsonConvert.SerializeObject(changedFiles, Formatting.Indented)), "json");
            return this.HtmlResult(html);
        }

        [HttpGet]
        [Route(GeneratorRoute.Scaffold)]
        public IActionResult Scaffold()
        {
            IList<string> changedFiles = ScaffoldingHelper.Scaffolding("Entity");
            string html = this.HigntLightHtml(HttpUtility.HtmlEncode(string.Join(Environment.NewLine, changedFiles)), "cs");
            return this.HtmlResult(html);
        }

        private string HigntLightHtml(string code, string type)
        {
            var css = ".hljs {display: block;overflow-x: auto;padding: 0.5em;background: white;color: black;}.hljs-comment,.hljs-quote,.hljs-variable {color: #008000;}.hljs-keyword,.hljs-selector-tag,.hljs-built_in,.hljs-name,.hljs-tag {color: #00f;}.hljs-string,.hljs-title,.hljs-section,.hljs-attribute,.hljs-literal,.hljs-template-tag,.hljs-template-variable,.hljs-type,.hljs-addition {color: #a31515;}.hljs-deletion,.hljs-selector-attr,.hljs-selector-pseudo,.hljs-meta {color: #2b91af;}.hljs-doctag {color: #808080;}.hljs-attr {color: #f00;}.hljs-symbol,.hljs-bullet,.hljs-link {color: #00b0e8;}.hljs-emphasis {font-style: italic;}.hljs-strong {font-weight: bold;}";
            var script1 = TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.HighlightJs));
            var script2 = TagHelper.Create(Tag.script, "hljs.initHighlightingOnLoad();");
            var cssTag = TagHelper.Create(Tag.style, css);
            var head = TagHelper.Create(Tag.head, cssTag, script1, script2);
            var codeTag = TagHelper.Create(Tag.code, new TagAttribute(Attr.Class, type), code);
            var pre = TagHelper.Create(Tag.pre, codeTag);
            var body = TagHelper.Create(Tag.body, pre);
            var html = TagHelper.Create(Tag.html, head, body);
            return TagHelper.ToHtml(html);
        }
    }
}
