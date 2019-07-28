namespace Quartz.Html
{
    using System;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Quartz.Files;
    using Quartz.Files.Css.Font.Awesome;
    using Quartz.Html.Tags;

    public static class HtmlHelper
    {
        private static TagHelperOutput GetLink(string href, string text)
        {
            var a = AspNetCore.Extensions.TagHelper.Create(Tag.a, new TagAttribute(Attr.Href, href), " " + text);
            return AspNetCore.Extensions.TagHelper.Create(Tag.li, a);
        }

        public static string GetFooter()
        {
            var li1 = GetLink("http://www.creative-tim.com", "Creative Tim");
            var li2 = GetLink("http://presentation.creative-tim.com", "About Us");
            var li3 = GetLink("https://github.com/188867052?tab=repositories", "Github");
            var li4 = GetLink("http://www.creative-tim.com/license", "Licenses");
            var ul = AspNetCore.Extensions.TagHelper.Create(Tag.ul, li1, li2, li3, li4);
            var nav = AspNetCore.Extensions.TagHelper.Create(Tag.nav, new TagAttribute(Attr.Class, "pull-left"), ul);
            return AspNetCore.Extensions.TagHelper.ToHtml(nav);
        }

        public static IHtmlContent GetBodyScript()
        {
            var script1 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.JqueryMinJs));
            var script2 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.BootstrapMinJs));
            var script3 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.MaterialMinJs));
            var script4 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.MomentMinJs));
            var script5 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.NouisliderMinJs));
            var script6 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.BootstrapDatetimepickerJs));
            var script7 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.BootstrapSelectpickerJs));
            var script8 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.BootstrapTagsinputJs));
            var script9 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.JasnyBootstrapMinJs));
            //var script10 = HtmlContent.TagHelper(Tag.script, new TagAttribute(Attr.Src, "https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_HERE")); ;
            var script11 = AspNetCore.Extensions.TagHelper.Create(Tag.script, new TagAttribute(Attr.Src, JsFile.MaterialKitJs));

            return AspNetCore.Extensions.TagHelper.Content(script1, script2, script3, script4, script5, script6, script7, script8, script9, /*script10,*/ script11);
        }

        public static IHtmlContent GetHeadCss()
        {
            var attribute0 = new TagAttributeList
            {
                { new TagAttribute(Attr.Href, "https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons") },
                { new TagAttribute(Attr.Rel, "stylesheet") },
            };
            var attribute1 = new TagAttributeList
            {
                { new TagAttribute(Attr.Href, CssFontAwesomeCssFile.FontAwesomeMinCss) },
                { new TagAttribute(Attr.Rel, "stylesheet") },
            };
            var attribute2 = new TagAttributeList
            {
                { new TagAttribute(Attr.Href, CssFile.BootstrapMinCss) },
                { new TagAttribute(Attr.Rel, "stylesheet") },
            };
            var attribute3 = new TagAttributeList
            {
                { new TagAttribute(Attr.Href, CssFile.MaterialKitCss) },
                { new TagAttribute(Attr.Rel, "stylesheet") },
            };

            var css0 = AspNetCore.Extensions.TagHelper.Create(Tag.link, attribute0);
            var css1 = AspNetCore.Extensions.TagHelper.Create(Tag.link, attribute1);
            var css2 = AspNetCore.Extensions.TagHelper.Create(Tag.link, attribute2);
            var css3 = AspNetCore.Extensions.TagHelper.Create(Tag.link, attribute3);

            return AspNetCore.Extensions.TagHelper.Content(css0, css1, css2, css3);
        }

        public static string GetCopyRight()
        {
            var attribute1 = new TagAttributeList
            {
                { new TagAttribute(Attr.Class, "copyright pull-right") },
                { new TagAttribute(Attr.Rel, "stylesheet") },
            };

            var i = AspNetCore.Extensions.TagHelper.Create(Tag.i, new TagAttribute(Attr.Class, "material-icons"), "favorite");
            var div = AspNetCore.Extensions.TagHelper.Create(Tag.div, attribute1);
            div.Content.AppendHtml($"Copyright &copy;{DateTime.Now.Year}, made with ");
            div.Content.AppendHtml(i);
            div.Content.AppendHtml(" by Harry Cheng for a better web. All Rights Reserved.");
            string copyRight = AspNetCore.Extensions.TagHelper.ToHtml(div);
            return copyRight;
        }
    }
}
