namespace Quartz.Html.GridColumn
{
    using System;
    using System.Linq.Expressions;
    using Quartz.Shared;

    public class TextGridColumn<T> : BaseGridColumn<T>
    {
        private readonly Expression<Func<T, string>> expression;

        public TextGridColumn(Expression<Func<T, string>> expression, string thead)
            : base(thead)
        {
            Check.NotNull(expression, nameof(expression));

            this.expression = expression;
        }

        public override string RenderTd(T entity)
        {
            Check.NotNull(entity, nameof(entity));

            var value = this.expression.Compile()(entity);
            if (this.MaxLength != 0 && value?.Length > this.MaxLength)
            {
                value = value.Substring(0, this.MaxLength);
            }

            return this.RenderTd(value);
        }
    }
}