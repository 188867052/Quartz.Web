namespace MaterialKit.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;

    public class Textarea
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "form-control"),
                new TagAttribute(Attr.Rows, "10"),
            };

            var h3 = TagHelper.Create(Tag.h3, "Textarea");
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "title"), h3);
            var label = TagHelper.Create(Tag.label, new TagAttribute(Attr.Class, "control-label"), " You can write your text here...");
            var textarea = TagHelper.Create(Tag.textarea, attributes);
            var div2 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "form-group label-floating"), label, textarea);

            return TagHelper.ToHtml(div, div2);
        }
    }
}
