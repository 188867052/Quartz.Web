namespace MaterialKit.Html.GridColumn
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AspNetCore.Extensions;
    using MaterialKit.Html.Buttons;
    using MaterialKit.Html.Inputs;
    using Microsoft.AspNetCore.Html;

    public abstract class GridConfiguration<T>
    {
        private readonly IList<T> entityList;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridConfiguration{T}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected GridConfiguration(IList<T> model)
        {
            this.entityList = model;
            this.Script = new List<IHtmlContent>();
        }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<IHtmlContent> Script { get; set; }

        public string GenerateButtons()
        {
            IList<SimpleButton> buttons = new List<SimpleButton>();
            this.CreateButtons(buttons);
            IList<IHtmlContent> list = new List<IHtmlContent>();
            foreach (var item in buttons)
            {
                list.Add(item.Render());
                this.Script = this.Script.Union(item.Script);
            }

            return TagHelper.ToHtml(list);
        }

        public string Render()
        {
            IList<BaseGridColumn<T>> gridColumns = new List<BaseGridColumn<T>>();
            this.CreateGridColumn(gridColumns);

            StringBuilder thead = new StringBuilder();
            thead.Append("<th  class=\"text-center\">#</th>");
            foreach (var item in gridColumns)
            {
                thead.Append(item.RenderTh());
            }

            StringBuilder tbody = new StringBuilder();
            foreach (var entity in this.entityList)
            {
                StringBuilder tr = new StringBuilder();
                int row = ((this.CurrentPage - 1) * this.PageSize) + this.entityList.IndexOf(entity) + 1;
                tr.Append($"<td  class=\"text-center\">{row}</td>");
                foreach (var item in gridColumns)
                {
                    tr.Append(item.RenderTd(entity));
                }

                tbody.Append($"<tr>{tr}</tr>");
            }

            var tableHtml = $"<div class=\"table-responsive\"><table class=\"table table-striped\"><thead><tr>{thead}</tr></thead><tbody>{tbody}</tbody></table></div>";
            var pagination = new Pagination().ToHtml();
            return tableHtml + pagination;
        }

        protected abstract void CreateGridColumn(IList<BaseGridColumn<T>> gridColumns);

        protected abstract void CreateButtons(IList<SimpleButton> buttons);
    }
}
