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

    public class FloatTextBox<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, string>> expression;
        private readonly Expression<Func<TModel, string>> modelExpression;
        private readonly string lable;

        public FloatTextBox(string lable, Expression<Func<TPostModel, string>> expression, Expression<Func<TModel, string>> modelExpression)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.lable = lable;
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
                { Attr.Value, value },
                { Attr.Name, name },
                { Attr.AutoComplete, "off" },
            };

            var label = TagHelper.Create(Tag.label, new TagAttribute(Attr.Class, "control-label"), this.lable);
            var input = TagHelper.Create(Tag.input, attributes);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "form-control-feedback"), new MaterialIcon("done").Html);
            return TagHelper.Div("form-group label-floating has-success", label, input, span);
        }
    }
}