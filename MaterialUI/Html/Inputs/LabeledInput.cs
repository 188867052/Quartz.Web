namespace MaterialUI.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;

    public class LabeledInput
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "email"),
                new TagAttribute(Attr.Class, "form-control"),
            };

            var input = TagHelper.Create(Tag.input, attributes);
            var label = TagHelper.Create(Tag.label, new TagAttribute(Attr.Class, "control-label"), "With Floating Label");
            input.TagMode = Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing;
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "form-group label-floating"), label, input);
            return TagHelper.ToHtml(div);
        }

        public static string Generate()
        {
            var div1 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "col-lg-3 col-sm-4"), new Input().ToHtml());
            var div2 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "col-lg-3 col-sm-4"), new LabeledInput().ToHtml());
            var div3 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "col-lg-3 col-sm-4"), new LabeledSuccessInput().ToHtml());
            var div4 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "col-lg-3 col-sm-4"), new LabeledErrorInput().ToHtml());
            var div5 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "col-lg-3 col-sm-4"), new MaterialIconInput().ToHtml());
            var div6 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "col-lg-3 col-sm-4"), new FontAwesomeIconInput().ToHtml());

            return TagHelper.ToHtml(div1, div2, div3, div4, div5, div6);
        }
    }
}
