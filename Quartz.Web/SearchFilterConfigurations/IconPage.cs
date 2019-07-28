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
               { Attr.DataParallax, "true" },
               { Attr.Style, "background-image: url('/img/examples/city.jpg');" },
            };
            var list = DapperExtension.FindAll<Icon>().OrderBy(o => o.Sort).ToList();
            TagAttributeList attributes = new TagAttributeList
            {
                { Attr.Class, "nav nav-pills nav-pills-icons" },
                { Attr.Role, "tablist" },
            };
            var ul = TagHelper.Create(Tag.ul, attributes, HtmlIcons.Generate(list));
            TagAttributeList attributes1 = new TagAttributeList
            {
                { Attr.Class, "col-md-12 col-lg-offset-0" },
                { Attr.Style, "margin-left: 20px;" },
            };
            var div = TagHelper.Create(Tag.div, attributes1, ul);
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
