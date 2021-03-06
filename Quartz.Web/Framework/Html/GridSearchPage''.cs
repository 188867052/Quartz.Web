namespace Quartz.Html
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Html.Buttons;
    using Quartz.Html.Icons;
    using Quartz.Html.Tags;
    using Quartz.SearchFilterConfigurations;
    using Quartz.ViewConfiguration;

    public abstract class GridSearchPage<TModel, TPostModel> : PageBase
    {
        private readonly IList<TModel> list;
        private readonly int index;
        private readonly int size;
        private readonly int total;

        protected GridSearchPage(IList<TModel> list, int index, int size, int total)
        {
            this.list = list;
            this.index = index;
            this.size = size;
            this.total = total;
        }

        protected override IHtmlContent InitJs()
        {
            var script = TagHelper.Create(
                Tag.script,
                this.RenderJavaScript() + "materialKit.initFormExtendedDatetimepickers();");
            return script;
        }

        protected IHtmlContent GenerateButtons()
        {
            IList<SimpleButton> buttons = new List<SimpleButton>();
            this.CreateButtons(buttons);
            IList<IHtmlContent> list = new List<IHtmlContent>();
            foreach (var item in buttons)
            {
                list.Add(item.Render());
                this.Script = this.Script.Union(item.Script);
            }

            return TagHelper.Combine(list);
        }

        protected IHtmlContent RenderFilter()
        {
            IList<IHtmlContent> contents = new List<IHtmlContent>();
            var columns = new List<LargeColumn<TModel, TPostModel>>();
            this.SearchFilterConfiguration.Render(columns);
            TModel model = default;
            foreach (var item in columns)
            {
                contents.Add(item.Render(model));
            }

            return TagHelper.Combine(contents);
        }

        protected IEnumerable<IHtmlContent> Script { get; set; } = new List<IHtmlContent>();

        protected override IHtmlContent Body()
        {
            var b = new TagAttributeList()
            {
               { Attr.Class, "page-header header-filter" },
               { Attr.DataParallax, "true" },
               { Attr.Style, "background-image: url('/img/examples/city.jpg');" },
            };
            var attributes = new TagAttributeList()
            {
                { Attr.Role, "button" },
                { Attr.DataToggle, "collapse" },
                { Attr.DataParent, "#accordion" },
                { Attr.Href, "#collapseOne" },
                { Attr.AriaExpanded, "false" },
                { Attr.AriaControls, "collapseOne" },
                { Attr.Class, "collapsed" },
            };
            var attributes2 = new TagAttributeList()
            {
                { Attr.Id, "collapseOne" },
                { Attr.Class, "panel-collapse collapse" },
                { Attr.Role, "tabpanel" },
                { Attr.AriaLabelledBy, "headingOne" },
                { Attr.AriaExpanded, "false" },
                { Attr.Style, "height: 0px;" },
            };
            var attributes1 = new TagAttributeList()
            {
                { Attr.Class, "panel-heading" },
                { Attr.Role, "tab" }, { Attr.Id, "headingOne" },
            };

            var colmd = TagHelper.Div("col-md-12 col-lg-offset-0 text-right", this.GenerateButtons());
            var panelbody = TagHelper.Div("panel-body", colmd, TagHelper.Create(Tag.form, this.RenderFilter()));
            var tabpanel = TagHelper.Create(Tag.div, attributes2, panelbody);
            var h4 = TagHelper.Create(Tag.h4, new TagAttribute(Attr.Class, "panel-title"), "Filter");
            h4.Content.AppendHtml(new MaterialIcon("keyboard_arrow_down").Html);
            var button = TagHelper.Create(Tag.a, attributes, h4);
            var heading = TagHelper.Create(Tag.div, attributes1, button);
            var panel = TagHelper.Div("panel panel-default", heading, tabpanel);
            var grid = TagHelper.Div("col-md-12 col-lg-offset-0 text-left", this.GenerateButtons(), this.GridConfiguration.Render(this.index, this.size, this.list, this.total));
            var div2 = TagHelper.Div("col-md-12 col-lg-offset-0", grid);
            var div1 = TagHelper.Div("col-md-12 col-lg-offset-0", panel);
            var row = TagHelper.Div("row", div1, div2);
            var container = TagHelper.Div("container", row);
            var profileContent = TagHelper.Div("profile-content", container);
            var mainRaised = TagHelper.Div("main main-raised", profileContent);
            var pageHeader = TagHelper.Create(Tag.div, b);
            var body = TagHelper.Create(Tag.body, new TagAttribute(Attr.Class, "profile-page"), Navigation.GetNavbar());
            body.Content.AppendHtml(pageHeader);
            body.Content.AppendHtml(mainRaised);
            body.Content.AppendHtml(this.Footer());
            return body;
        }

        protected abstract GridConfigurationBase<TModel> GridConfiguration { get; }

        protected abstract SearchFilterConfigurationBase<TModel, TPostModel> SearchFilterConfiguration { get; }

        protected abstract void CreateButtons(IList<SimpleButton> buttons);
    }
}
