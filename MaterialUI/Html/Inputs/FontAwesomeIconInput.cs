namespace MaterialKit.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;

    public class FontAwesomeIconInput
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "text"),
                new TagAttribute(Attr.Class, "form-control"),
                new TagAttribute(Attr.Placeholder, "With Font Awesome Icons"),
            };

            var input = TagHelper.Create(Tag.input, attributes);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "input-group-addon"), new FontAwesomeIcon().Html);
            input.TagMode = Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing;
            var div = TagHelper.Div("input-group", span, input);
            return TagHelper.ToHtml(div);
        }
    }
}
