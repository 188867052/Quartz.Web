namespace MaterialUI.Html.TextBoxs
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Html.Icons;
    using MaterialUI.Html.Tags;
    using MaterialUI.Javascript;
    using Microsoft.AspNetCore.Html;

    public class IntegarIconTextBox<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, int?>> expression;
        private readonly Expression<Func<TModel, int?>> modelExpression;
        private readonly string placeholder;

        public IntegarIconTextBox(string placeholder, Expression<Func<TPostModel, int?>> expression, Expression<Func<TModel, int?>> modelExpression = null)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.placeholder = placeholder;
        }

        public ComulnWidth Width { get; set; } = ComulnWidth.Default;

        public IHtmlContent Render(TModel entity)
        {
            string id = new Identifier().Value;
            int? value = default;
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