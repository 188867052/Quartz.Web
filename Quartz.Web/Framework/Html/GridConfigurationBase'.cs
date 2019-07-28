namespace Quartz.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Newtonsoft.Json;
    using Quartz.Html.GridColumn;
    using Quartz.Html.Inputs;
    using Quartz.Html.Tags;
    using Quartz.Shared;

    public abstract class GridConfigurationBase<TModel>
    {
        public bool EnablePagination { get; set; } = true;

        public abstract string GridStateChange { get; }

        public abstract object Data { get; }

        public IHtmlContent Render(int index, int size, IList<TModel> list, int total)
        {
            Check.NotEmpty(list.ToList(), nameof(list));

            IList<BaseGridColumn<TModel>> gridColumns = new List<BaseGridColumn<TModel>>();
            this.CreateGridColumn(gridColumns);

            var thead = TagHelper.Create(Tag.th, new TagAttribute(Attr.Class, "text-center"), "#");
            foreach (var item in gridColumns)
            {
                thead.Content.AppendHtml(item.RenderTh());
            }

            var tbody = TagHelper.Create(Tag.tbody);
            foreach (var entity in list)
            {
                int row = ((index - 1) * size) + list.IndexOf(entity) + 1;
                var tr = TagHelper.Create(Tag.td, new TagAttribute(Attr.Class, "text-center"), row.ToString());
                foreach (var item in gridColumns)
                {
                    tr.Content.AppendHtml(item.RenderTd(entity));
                }

                tbody.Content.AppendHtml(TagHelper.Create(Tag.tr, tr));
            }

            var theadTag = TagHelper.Create(Tag.thead, TagHelper.Create(Tag.tr, thead));
            var table = TagHelper.Create(Tag.table, new TagAttribute(Attr.Class, "table table-striped"), theadTag, tbody);
            var responsiveTable = TagHelper.Div("table-responsive", table);
            responsiveTable.Attributes.Add(Attr.Url, this.GridStateChange);
            responsiveTable.Attributes.Add(Attr.Data, JsonConvert.SerializeObject(this.Data));
            if (this.EnablePagination)
            {
                responsiveTable.PostElement.AppendHtml(new Pagination().Render(index, size, total));
            }

            return responsiveTable;
        }

        protected abstract void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns);
    }
}
