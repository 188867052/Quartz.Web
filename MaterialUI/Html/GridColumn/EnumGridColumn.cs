namespace MaterialKit.Html.GridColumn
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;

    public class EnumGridColumn<T> : BaseGridColumn<T>
    {
        private readonly Expression<Func<T, Enum>> expression;

        public EnumGridColumn(Expression<Func<T, Enum>> expression, string thead)
            : base(thead)
        {
            Check.NotNull(expression, nameof(expression));

            this.expression = expression;
        }

        public override string RenderTd(T entity)
        {
            Check.NotNull(entity, nameof(entity));

            Enum value = this.expression.Compile()(entity);
            string display = value.ToString();
            return this.RenderTd(display);
        }
    }
}