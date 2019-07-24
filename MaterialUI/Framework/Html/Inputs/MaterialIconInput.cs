namespace MaterialUI.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Icons;
    using MaterialUI.Html.Tags;

    public class MaterialIconInput
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "text"),
                new TagAttribute(Attr.Class, "form-control"),
                new TagAttribute(Attr.Placeholder, "With Material Icons"),
            };

            var input = TagHelper.Create(Tag.input, attributes);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "input-group-addon"), new MaterialIcon("group").Html);
            input.TagMode = Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing;
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "input-group"), span, input);
            return TagHelper.ToHtml(div);
        }
    }
}
