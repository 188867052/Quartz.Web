namespace MaterialUI.Html.GridColumn
{
    using AspNetCore.Extensions;

    public abstract class BaseGridColumn<T>
    {
        protected BaseGridColumn(string thead)
        {
            Check.NotEmpty(thead, nameof(thead));

            this.Thead = thead;
        }

        protected string Thead { get; }

        public virtual string RenderTh()
        {
            return $"<th>{this.Thead}</th>";
        }

        public int MaxLength { get; set; }

        public abstract string RenderTd(T entity);

        protected virtual string RenderTd(object value)
        {
            return $"<td>{value}</td>";
        }
    }
}