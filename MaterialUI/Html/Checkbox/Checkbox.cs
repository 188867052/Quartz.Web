namespace MaterialUI.Html.Checkbox
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;

    public class Checkbox
    {
        private readonly string text;
        private readonly bool isChecked;
        private readonly bool isDisabled;

        public Checkbox(string text, bool isChecked = default, bool isDisabled = default)
        {
            this.text = text;
            this.isChecked = isChecked;
            this.isDisabled = isDisabled;
        }

        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "checkbox"),
                new TagAttribute(Attr.Name, "optionsCheckboxes"),
            };
            if (this.isChecked)
            {
                attributes.Add(new TagAttribute(Attr.Checked));
            }

            if (this.isDisabled)
            {
                attributes.Add(new TagAttribute(Attr.Disabled));
            }

            var input = TagHelper.SelfClosingTag(Tag.input, attributes);
            input.PostElement.SetContent(this.text);
            var label = TagHelper.Create(Tag.label, input);
            var div = TagHelper.Div("checkbox", label);
            return TagHelper.ToHtml(div);
        }

        public static string Generate()
        {
            var div1 = new Checkbox("Unchecked");
            var div2 = new Checkbox("Checked", true);
            var div3 = new Checkbox("Disabled Unchecked", isDisabled: true);
            var div4 = new Checkbox("Disabled Checked", true, true);

            return div1.ToHtml() + div2.ToHtml() + div3.ToHtml() + div4.ToHtml();
        }
    }
}
