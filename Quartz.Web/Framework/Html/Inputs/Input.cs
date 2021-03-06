namespace Quartz.Html.Inputs
{
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Quartz.Html.Tags;

    public class Input
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "text"),
                new TagAttribute(Attr.Value, ""),
                new TagAttribute(Attr.Placeholder, "Regular"),
                new TagAttribute(Attr.Class, "form-control"),
            };

            var input = AspNetCore.Extensions.TagHelper.Create(Tag.input, attributes);
            input.TagMode = TagMode.SelfClosing;
            var div = AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "form-group"), input);
            return AspNetCore.Extensions.TagHelper.ToHtml(div);
        }
    }
}
