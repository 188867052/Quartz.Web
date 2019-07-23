namespace MaterialKit.ViewConfiguration
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using MaterialKit.Html.GridColumn;
    using MaterialKit.Html.Tags;
    using MaterialKit.Javascript;
    using Microsoft.AspNetCore.Html;

    public class CheckBox<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, bool?>> expression;
        private readonly Expression<Func<TModel, bool?>> modelExpression;
        private readonly string text;

        public CheckBox(string text, Expression<Func<TModel, bool?>> modelExpression, Expression<Func<TPostModel, bool?>> expression)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.text = text;
        }

        public ComulnWidth Width { get; set; } = default;

        public IHtmlContent Render(TModel entity)
        {
            string id = new Identifier().Value;
            bool? value = default;
            string name = this.expression.GetPropertyName();
            if (entity != null)
            {
                value = this.modelExpression.Compile()(entity);
            }

            TagAttributeList attributes = new TagAttributeList
            {
                { Attr.Type, "checkbox" },
                { Attr.Name, name },
            };

            if (value.HasValue && value.Value)
            {
                attributes.Add(new TagAttribute(Attr.Checked));
            }

            var input = TagHelper.Create(Tag.input, attributes);
            var label = TagHelper.Create(Tag.label, input);
            label.PostElement.AppendHtml(this.text);

            return TagHelper.Div("checkbox", label);
        }
    }
}