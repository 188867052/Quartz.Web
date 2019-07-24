namespace MaterialUI.ViewConfiguration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Html.Tags;
    using MaterialUI.Javascript;
    using MaterialUI.Shared;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Newtonsoft.Json;

    public class SingleSelect<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        private readonly Expression<Func<TPostModel, object>> expression;
        private readonly Expression<Func<TModel, object>> modelExpression;
        private readonly string placeholder;
        private readonly IList<TagHelperOutput> options;

        public SingleSelect(string placeholder, Expression<Func<TPostModel, object>> expression, Expression<Func<TModel, object>> modelExpression)
        {
            this.expression = expression;
            this.modelExpression = modelExpression;
            this.placeholder = placeholder;
            this.options = new List<TagHelperOutput>();
        }

        public ComulnWidth Width { get; set; } = ComulnWidth.Default;

        public string Function { get; set; }

        public string Url { get; set; }

        public bool Init { get; set; }

        public object Data { get; set; }

        public Identifier Id { get; set; }

        public void AddOption(object key, string text)
        {
            var option = AspNetCore.Extensions.TagHelper.Create(Tag.option, new TagAttribute(Attr.Value, key), text);
            this.options.Add(option);
        }

        public virtual IHtmlContent Render(TModel entity)
        {
            Check.NotEmpty(this.options.ToList(), nameof(this.options));

            object value = default;
            string name = this.expression.GetPropertyName();

            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "selectpicker"),
                new TagAttribute(Attr.DataStyle, "btn btn-primary btn-round"),
                new TagAttribute(Attr.DataSize, this.options.Count),
                new TagAttribute(Attr.Name, name),
            };

            if (entity != null)
            {
                value = this.modelExpression.Compile()(entity);
                if (value != default)
                {
                    var option = this.options.FirstOrDefault(o => o.Attributes.FirstOrDefault(a => a.Name == Attr.Value).Value.ToString() == value.ToString());
                    if (option != null)
                    {
                        option.Attributes.Add(new TagAttribute(Attr.Selected));
                    }
                }
            }

            var select = AspNetCore.Extensions.TagHelper.Create(Tag.select, attributes, this.options);
            if (!string.IsNullOrEmpty(this.Url))
            {
                Check.NotNull(this.Id, nameof(this.Id));
                select.Attributes.Add(Attr.Url, this.Url);
                select.Attributes.Add(Attr.Id, this.Id);
                select.Attributes.Add(Attr.Data, JsonConvert.SerializeObject(this.Data));
            }

            if (!string.IsNullOrEmpty(this.Function))
            {
                string script = new JavaScriptEvent(this.Function, this.Id, JavaScriptEventEnum.Change).Render();
                select.PostElement.AppendHtml(script);
            }

            if (this.Init)
            {
                select.PostElement.AppendHtml("<script>$('.selectpicker').selectpicker('selectAll');</script>");
            }

            return select;
        }
    }
}