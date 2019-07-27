namespace MaterialUI.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Html.Inputs;
    using MaterialUI.Html.Tags;
    using Microsoft.AspNetCore.Html;
    using Newtonsoft.Json;

    public abstract class GridConfigurationBase<TModel>
    {
        public bool EnablePagination { get; set; } = true;

        public abstract string GridStateChange { get; }
        public abstract object Data { get; }

        public IHtmlContent Render(int index, int size, IList<TModel> list, int total)
        {
            IList<BaseGridColumn<TModel>> gridColumns = new List<BaseGridColumn<TModel>>();
            this.CreateGridColumn(gridColumns);

            var thead = TagHelper.Create("th", new TagAttribute("class", "text-center"), "#");
            foreach (var item in gridColumns)
            {
                thead.Content.AppendHtml(item.RenderTh());
            }

            var tbody = TagHelper.Create(Tag.tbody);
            foreach (var entity in list)
            {
                int row = ((index - 1) * size) + list.IndexOf(entity) + 1;
                var tr = TagHelper.Create("td", new TagAttribute("class", "text-center"), row.ToString());
                foreach (var item in gridColumns)
                {
                    tr.Content.AppendHtml(item.RenderTd(entity));
                }

                tbody.Content.AppendHtml(TagHelper.Create("tr", tr));
            }

            var theadTag = TagHelper.Create(Tag.thead, TagHelper.Create(Tag.tr, thead));
            var table = TagHelper.Create(Tag.table, new TagAttribute(Attr.Class, "table table-striped"), theadTag, tbody);
            var responsiveTable = TagHelper.Div("table-responsive", table);
            responsiveTable.Attributes.Add("url", this.GridStateChange);
            responsiveTable.Attributes.Add("data", JsonConvert.SerializeObject(this.Data));
            if (this.EnablePagination)
            {
                responsiveTable.PostElement.AppendHtml(new Pagination().ToHtml(index, size, total));
            }

            //responsiveTable.PostContent.AppendHtml(TagHelper.Script(this.Script));
            return responsiveTable;
        }

        private string Script = "framework.pageInit();";
        protected abstract void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns);
    }
}
