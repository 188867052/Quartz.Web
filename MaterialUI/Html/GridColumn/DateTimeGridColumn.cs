namespace MaterialKit.Html.GridColumn
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;

    public class DateTimeGridColumn<T> : BaseGridColumn<T>
    {
        private readonly Expression<Func<T, DateTime?>> expression;

        public DateTimeGridColumn(Expression<Func<T, DateTime?>> expression, string thead)
            : base(thead)
        {
            Check.NotNull(expression, nameof(expression));

            this.expression = expression;
        }

        public override string RenderTd(T entity)
        {
            Check.NotNull(entity, nameof(entity));

            var value = this.expression.Compile()(entity);
            return this.RenderTd(value);
        }
    }
}