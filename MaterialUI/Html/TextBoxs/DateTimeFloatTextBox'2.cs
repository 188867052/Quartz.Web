namespace MaterialKit.Html.TextBoxs
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using MaterialKit.Html.GridColumn;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;
    using MaterialKit.Javascript;
    using Microsoft.AspNetCore.Html;

    public class DateTimeFloatTextBox<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, DateTime?>> expression;
        private readonly Expression<Func<TModel, DateTime?>> modelExpression;
        private readonly string lable;

        public DateTimeFloatTextBox(string lable, Expression<Func<TPostModel, DateTime?>> expression, Expression<Func<TModel, DateTime?>> modelExpression = null)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.lable = lable;
        }

        public DatePickerType Type { get; set; } = DatePickerType.DateTime;

        public ComulnWidth Width { get; set; } = ComulnWidth.Default;

        public IHtmlContent Render(TModel entity)
        {
            string id = new Identifier().Value;
            DateTime? value = default;
            string name = this.expression.GetPropertyName();
            var attributes = new TagAttributeList
            {
                { Attr.Type, "text" },
                { Attr.Class, $"form-control {this.Type.ToString().ToLower()}picker" },
                { Attr.Name, name },
                { Attr.Id, id },
                { Attr.AutoComplete, "off" },
            };

            if (entity != null)
            {
                value = this.modelExpression.Compile()(entity);
                if (value.HasValue && value != null)
                {
                    attributes.Add(Attr.Value, value);
                }
            }

            var label = TagHelper.Create(Tag.label, new TagAttribute(Attr.Class, "control-label"), this.lable);
            var input = TagHelper.Create(Tag.input, attributes);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "form-control-feedback"), new MaterialIcon("done").Html);//  clear
            return TagHelper.Div("form-group label-floating has-success", label, input, span);// has-error
        }
    }

    public enum DatePickerType
    {
        DateTime = 0,
        Date = 1,
        Time = 2,
    }
}