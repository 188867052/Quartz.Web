namespace Quartz.Html.Inputs
{
    using AspNetCore.Extensions;
    using Quartz.Html.Tags;

    public class LabeledErrorInput
    {
        public string ToHtml()
        {
            TagAttributeList attributes2 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "email"),
                new TagAttribute(Attr.Value, "Error Input"),
                new TagAttribute(Attr.Class, "form-control"),
            };

            var input = TagHelper.Create(Tag.input, attributes2);
            var span = TagHelper.Create(Tag.span, new TagAttribute(Attr.Class, "material-icons form-control-feedback"), "clear");
            var label = TagHelper.Create(Tag.label, new TagAttribute(Attr.Class, "control-label"), "Error input");
            input.TagMode = Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing;
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "form-group label-floating has-error"), label, input, span);
            return TagHelper.ToHtml(div);
        }
    }
}
