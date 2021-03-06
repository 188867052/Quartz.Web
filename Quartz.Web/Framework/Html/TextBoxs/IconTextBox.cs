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

    public class IconTextBox<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, string>> expression;
        private readonly Expression<Func<TModel, string>> modelExpression;
        private readonly string placeholder;

        public IconTextBox(string placeholder, Expression<Func<TPostModel, string>> expression, Expression<Func<TModel, string>> modelExpression = null)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.placeholder = placeholder;
        }

        public ComulnWidth Width { get; set; } = ComulnWidth.Default;

        public IHtmlContent Render(TModel entity)
        {
            string id = new Identifier().Value;
            string value = default;
            string name = this.expression.GetPropertyName();
            if (entity != null)
            {
                value = this.modelExpression.Compile()(entity);
            }

            TagAttributeList attributes = new TagAttributeList
            {
                { Attr.Type, "text" },
                { Attr.Class, "form-control" },
                { Attr.Placeholder, this.placeholder },
                { Attr.Name, name },
                { Attr.Value, value },
                { Attr.Id, id },
            };

            var input = TagHelper.Create(Tag.input, attributes);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "input-group-addon"), new MaterialIcon("face").Html);
            span.PostElement.SetHtmlContent(input);
            return TagHelper.Div("input-group", span);
        }
    }
}