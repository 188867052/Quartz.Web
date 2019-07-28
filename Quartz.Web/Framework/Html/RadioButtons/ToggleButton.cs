namespace Quartz.Html.RadioButtons
{
    using AspNetCore.Extensions;
    using Quartz.Html.Tags;

    public class ToggleButton
    {
        private readonly string text;
        private readonly bool isChecked;
        private readonly bool isDisabled;

        public ToggleButton(string text, bool isChecked = default, bool isDisabled = default)
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
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "togglebutton"), label);
            return TagHelper.ToHtml(div);
        }

        public static string Generate()
        {
            var div1 = new ToggleButton("Toggle is off");
            var div2 = new ToggleButton("Toggle is on", true);
            var div3 = new ToggleButton("Disabled Toggle is off", isDisabled: true);
            var div4 = new ToggleButton("Disabled Toggle is on", true, true);

            return div1.ToHtml() + div2.ToHtml() + div3.ToHtml() + div4.ToHtml();
        }
    }
}
