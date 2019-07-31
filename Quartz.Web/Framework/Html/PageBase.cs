namespace Quartz.Html
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Files;
    using Quartz.Files.Css.Font.Awesome;
    using Quartz.Html.Tags;
    using Quartz.Javascript;

    public abstract class PageBase
    {
        private static IHtmlContent footer;

        protected virtual string Title { get; } = "Task Schedule";

        protected abstract IHtmlContent InitJs();

        protected abstract IHtmlContent Body();

        public string Render()
        {
            var html = TagHelper.Create(Tag.html, new TagAttribute("lang", "en"), this.Header(), this.Body(), this.InitJs());
            var text = TagHelper.ToHtml(html);
            return "<!doctype html>" + text;
        }

        /// <summary>
        /// Css文件.
        /// </summary>
        /// <returns>css list.</returns>
        protected abstract IList<string> CssFiles();

        /// <summary>
        /// JavaScript文件.
        /// </summary>
        /// <returns>The list.</returns>
        protected abstract IList<string> JavaScriptFiles();

        protected abstract IList<ViewInstanceConstruction> CreateViewInstanceConstructions();

        protected virtual IHtmlContent Header()
        {
            IList<IHtmlContent> content = new List<IHtmlContent>
            {
                TagHelper.SelfClosingTag(Tag.link, new TagAttributeList() { { Attr.Rel, "apple-touch-icon" }, { Attr.Sizes, "sizes" }, { Attr.Href, "/img/apple-icon.png" }, }),
                TagHelper.SelfClosingTag(Tag.link, new TagAttributeList() { { Attr.Rel, "icon" }, { Attr.Type, "image/png" }, { Attr.Href, "/img/favicon.png" }, }),
                TagHelper.Create(Tag.meta, new TagAttributeList { { "http-equiv", "X-UA-Compatible" }, { "content", "IE=edge,chrome=1" }, }),
                TagHelper.Create(Tag.title, this.Title),
                TagHelper.Create(Tag.meta, new TagAttributeList { { "content", "width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" }, { Attr.Name, "viewport" }, }),
            };

            content = content.Concat(this.CssResource()).Concat(this.JavaScriptResource()).ToList();
            return TagHelper.Create(Tag.head, content.ToArray());
        }

        protected string RenderJavaScript()
        {
            var viewInstances = this.CreateViewInstanceConstructions();
            viewInstances.Add(new FrameworkViewInstance());
            return viewInstances.Aggregate<ViewInstanceConstruction, string>(default, (current, instance) => current + instance.ViewInstance().Render());
        }

        private IEnumerable<IHtmlContent> JavaScriptResource()
        {
            List<string> list = new List<string>
            {
                JsFile.JqueryMinJs,
                JsFile.BootstrapMinJs,
                JsFile.MaterialMinJs,
                JsFile.MomentMinJs,
                JsFile.NouisliderMinJs,
                JsFile.BootstrapDatetimepickerJs,
                JsFile.BootstrapSelectpickerJs,
                JsFile.BootstrapTagsinputJs,
                JsFile.JasnyBootstrapMinJs,
                JsFile.MaterialKitJs,
                JsFile.FrameworkJs,
            };

            list.AddRange(this.JavaScriptFiles());
            IList<IHtmlContent> content = new List<IHtmlContent>();
            foreach (var item in list)
            {
                content.Add(TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, item)));
            }

            return content;
        }

        private IEnumerable<IHtmlContent> CssResource()
        {
            List<string> list = new List<string>
            {
                "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons",
                CssFontAwesomeCssFile.FontAwesomeMinCss,
                CssFile.BootstrapMinCss,
                CssFile.MaterialKitCss,
            };

            list.AddRange(this.CssFiles());
            IList<IHtmlContent> content = new List<IHtmlContent>();
            foreach (var item in list)
            {
                content.Add(TagHelper.SelfClosingTag(Tag.link, new TagAttributeList { { new TagAttribute(Attr.Href, item) }, { new TagAttribute(Attr.Rel, "stylesheet") }, }));
            }

            return content;
        }

        protected IHtmlContent Footer()
        {
            if (footer != null)
            {
                return footer;
            }

            var container = TagHelper.Div("container", HtmlHelper.GetFooter() + HtmlHelper.GetCopyRight());
            footer = TagHelper.Create(Tag.footer, new TagAttribute(Attr.Class, "footer"), container);
            return footer;
        }
    }
}
