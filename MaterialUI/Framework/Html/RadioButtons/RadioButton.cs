namespace MaterialUI.Html.RadioButtons
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;

    public class RadioButton
    {
        private readonly string text;
        private readonly string name;
        private readonly bool isChecked;
        private readonly bool isDisabled;

        public RadioButton(string text, string name, bool isChecked = default, bool isDisabled = default)
        {
            this.text = text;
            this.name = name;
            this.isChecked = isChecked;
            this.isDisabled = isDisabled;
        }

        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "radio"),
                new TagAttribute(Attr.Name, this.name),
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
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "radio"), label);
            return TagHelper.ToHtml(div);
        }

        public static string Generate()
        {
            var div1 = new RadioButton("Radio is off", "Test");
            var div2 = new RadioButton("Radio is on", "Test", true);
            var div3 = new RadioButton("Disabled Radio is off", "Test1", isDisabled: true);
            var div4 = new RadioButton("Disabled Radio is on", "Test2", true, true);

            return div1.ToHtml() + div2.ToHtml() + div3.ToHtml() + div4.ToHtml();
        }
    }
}
