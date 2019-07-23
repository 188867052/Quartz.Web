namespace MaterialUI.Html.GridColumn
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;

    public class IconGridColumn<T> : BaseGridColumn<T>
    {
        private readonly Expression<Func<T, string>> expression;

        public IconGridColumn(Expression<Func<T, string>> expression, string thead)
            : base(thead)
        {
            Check.NotNull(expression, nameof(expression));

            this.expression = expression;
        }

        public override string RenderTd(T entity)
        {
            Check.NotNull(entity, nameof(entity));

            var value = this.expression.Compile()(entity);
            var innerHtml = $"<span class=\"{value}\"></span>";
            return this.RenderTd(innerHtml);
        }
    }
}