namespace MaterialUI.ViewConfiguration
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Html.Tags;
    using MaterialUI.Javascript;
    using Microsoft.AspNetCore.Html;

    public class FooterButton<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, object>> expression;
        private readonly Expression<Func<TModel, object>> modelExpression;
        private readonly string lableText;
        private readonly string url;

        public FooterButton(string url, Expression<Func<TModel, object>> modelExpression, Expression<Func<TPostModel, object>> expression, string lableText)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.lableText = lableText;
            this.url = url;
        }

        public ComulnWidth Width { get; set; }

        public IHtmlContent Render(TModel entity)
        {
            string id = new Identifier().Value;
            object value = default;
            string name = this.expression.GetPropertyName();
            if (entity != null)
            {
                value = this.modelExpression.Compile()(entity);
            }

            TagAttributeList attributes = new TagAttributeList
            {
                { Attr.Href, "#pablo" },
                { Attr.Class, "btn btn-primary btn-round" },
                { Attr.Type, "submit" },
                { Attr.Name, name },
                { Attr.Value, value },
                { Attr.Url, this.url },
            };

            var a = TagHelper.Create(Tag.a, attributes, this.lableText);
            return TagHelper.Div("modal-footer text-center", a);
        }
    }
}