namespace Quartz.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Dapper;
    using Quartz.Entity;
    using Quartz.Html;
    using Quartz.Html.Inputs;
    using Quartz.Html.Tags;
    using Quartz.Javascript;

    public class IconPage : PageBase
    {
        protected override IList<ViewInstanceConstruction> CreateViewInstanceConstructions()
        {
            return new List<ViewInstanceConstruction>();
        }

        protected override IList<string> CssFiles()
        {
            return new List<string>();
        }

        protected override IHtmlContent InitJs()
        {
            return TagHelper.Empty;
        }

        protected override IList<string> JavaScriptFiles()
        {
            return new List<string>();
        }

        protected override IHtmlContent Body()
        {
            var b = new TagAttributeList()
            {
               { Attr.Class, "page-header header-filter" },
               { "data-parallax", "true" },
               { Attr.Style, "background-image: url('/img/examples/city.jpg');" },
            };
            var list = DapperExtension.FindAll<Icon>().ToList();
            var ul = TagHelper.Create(Tag.ul, new TagAttributeList { { "class", "nav nav-pills nav-pills-icons" }, { "role", "tablist" } }, HtmlIcons.Generate(list));
            var div = TagHelper.Create(Tag.div, new TagAttributeList { { "class", "col-md-12 col-lg-offset-0" }, { "style", "margin-left: 20px;" } }, ul);
            var pageHeader = TagHelper.Create(Tag.div, b);
            var container = TagHelper.Div("container", div);
            var mainRaised = TagHelper.Div("main main-raised", container);

            var body = TagHelper.Create(Tag.body, new TagAttribute(Attr.Class, "profile-page"), Navigation.GetNavbar());
            body.Content.AppendHtml(pageHeader);
            body.Content.AppendHtml(mainRaised);
            body.Content.AppendHtml(this.Footer());

            return body;
        }
    }
}
