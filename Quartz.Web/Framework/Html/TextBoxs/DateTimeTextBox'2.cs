namespace Quartz.Html.TextBoxs
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Html.GridColumn;
    using Quartz.Html.Icons;
    using Quartz.Html.Tags;
    using Quartz.Javascript;

    public class DateTimeTextBox<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, DateTime?>> expression;
        private readonly Expression<Func<TModel, DateTime?>> modelExpression;
        private readonly string lable;

        public DateTimeTextBox(string lable, Expression<Func<TPostModel, DateTime?>> expression, Expression<Func<TModel, DateTime?>> modelExpression = null)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.lable = lable;
        }

        public ComulnWidth Width { get; set; } = ComulnWidth.Default;

        public IHtmlContent Render(TModel entity)
        {
            string id = new Identifier().Value;
            DateTime? value = default;
            string name = this.expression.GetPropertyName();
            if (entity != null)
            {
                value = this.modelExpression.Compile()(entity);
            }

            var attributes = new TagAttributeList
            {
                { Attr.Type, "text" },
                { Attr.Class, "form-control datetimepicker" },
                { Attr.Name, name },
                { Attr.Placeholder, this.lable },
                { Attr.Value, value.Value },
                { Attr.Id, id },
            };

            var input = TagHelper.Create(Tag.input, attributes);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "input-group-addon"), new MaterialIcon("access_time").Html);
            span.PostElement.SetHtmlContent(input);
            return TagHelper.Div("input-group", span);
        }
    }
}