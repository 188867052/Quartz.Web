namespace Quartz.Html.Inputs
{
    using AspNetCore.Extensions;
    using Quartz.Html.Icons;
    using Quartz.Html.Tags;

    public class LabeledSuccessInput
    {
        public string ToHtml()
        {
            TagAttributeList attributes2 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "text"),
                new TagAttribute(Attr.Value, "Success"),
                new TagAttribute(Attr.Class, "form-control"),
            };

            var input = TagHelper.Create(Tag.input, attributes2);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "form-control-feedback"), new MaterialIcon("done").Html);
            var label = TagHelper.Create(Tag.label, new TagAttribute(Attr.Class, "control-label"), "Success input");
            input.TagMode = Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing;
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "form-group label-floating has-success"), label, input, span);
            return TagHelper.ToHtml(div);
        }
    }
}
