namespace MaterialUI.Html
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AspNetCore.Extensions;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Html.Icons;
    using MaterialUI.Html.Inputs;
    using MaterialUI.Html.Tags;
    using MaterialUI.ViewConfiguration;
    using Microsoft.AspNetCore.Html;

    public abstract class GridSearchPage<TModel, TPostModel> : PageBase
    {
        protected override IHtmlContent InitJs()
        {
            var script = TagHelper.Create(Tag.script, this.RenderJavaScript() + "materialKit.initFormExtendedDatetimepickers();");
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
            this.Filter(columns);
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
               { "data-parallax", "true" },
               { Attr.Style, "background-image: url('/img/examples/city.jpg');" },
            };
            TagAttributeList attributes = new TagAttributeList
            {
                { "role", "button" },
                { "data-toggle", "collapse" },
                { "data-parent", "#accordion" },
                { "href", "#collapseOne" },
                { "aria-expanded", "false" },
                { "aria-controls", "collapseOne" },
                { "class", "collapsed" },
            };
            TagAttributeList attributes2 = new TagAttributeList
            {
                { "id", "collapseOne" },
                { "class", "panel-collapse collapse" },
                { "role", "tabpanel" },
                { "aria-labelledby", "headingOne" },
                { "aria-expanded", "false" },
                { "style", "height: 0px;" },
            };

            var colmd = TagHelper.Div("col-md-12 col-lg-offset-0 text-right", this.GenerateButtons());
            var panelbody = TagHelper.Div("panel-body", colmd, TagHelper.Create(Tag.form, this.RenderFilter()));
            var tabpanel = TagHelper.Create(Tag.div, attributes2, panelbody);
            var h4 = TagHelper.Create(Tag.h4, new TagAttribute(Attr.Class, "panel-title"), "Filter");
            h4.Content.AppendHtml(new MaterialIcon("keyboard_arrow_down").Html);
            var button = TagHelper.Create(Tag.a, attributes, h4);
            var heading = TagHelper.Create(Tag.div, new TagAttributeList { { "class", "panel-heading" }, { "role", "tab" }, { "id", "headingOne" }, }, button);
            var panel = TagHelper.Div("panel panel-default", heading, tabpanel);
            var grid = TagHelper.Div("col-md-12 col-lg-offset-0 text-left", this.GenerateButtons(), this.RenderGridColumn());
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

        protected abstract void Filter(IList<LargeColumn<TModel, TPostModel>> columns);

        protected abstract void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns);

        private readonly int currentPage = 1;
        private readonly int pageSize = 10;
        private IList<TModel> list;

        protected GridSearchPage(IList<TModel> list)
        {
            this.list = list;
        }

        private IHtmlContent RenderGridColumn()
        {
            IList<BaseGridColumn<TModel>> gridColumns = new List<BaseGridColumn<TModel>>();
            this.CreateGridColumn(gridColumns);

            StringBuilder thead = new StringBuilder();
            thead.Append("<th  class=\"text-center\">#</th>");
            foreach (var item in gridColumns)
            {
                thead.Append(item.RenderTh());
            }

            StringBuilder tbody = new StringBuilder();
            foreach (var entity in this.list)
            {
                StringBuilder tr = new StringBuilder();
                int row = (this.currentPage - 1) * this.pageSize + this.list.IndexOf(entity) + 1;
                tr.Append($"<td  class=\"text-center\">{row}</td>");
                foreach (var item in gridColumns)
                {
                    tr.Append(item.RenderTd(entity));
                }

                tbody.Append($"<tr>{tr}</tr>");
            }

            var theadTag = TagHelper.Create(Tag.thead, TagHelper.Create(Tag.tr, thead.ToString()));
            var tbodyTag = TagHelper.Create(Tag.tbody, tbody.ToString());
            var table = TagHelper.Create(Tag.table, new TagAttribute(Attr.Class, "table table-striped"), theadTag, tbodyTag);
            var responsive = TagHelper.Div("table-responsive", table);
            responsive.PostElement.AppendHtml(new Pagination().ToHtml());
            return responsive;
        }

        protected abstract void CreateButtons(IList<SimpleButton> buttons);
    }
}
