namespace MaterialKit.Html.DropDowns
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;

    public class DropDownToggle
    {
        public DropDownToggle(string iconClass, string text)
        {
            this.IconClass = iconClass;
            this.Text = text;
        }

        public string IconClass { get; set; }

        public string Text { get; set; }

        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList
            {
                new TagAttribute(Attr.Href, "#"),
                new TagAttribute(Attr.Class, "dropdown-toggle"),
                new TagAttribute(Attr.DataToggle, "dropdown"),
            };
            var i = TagHelper.Create(Tag.i, new TagAttribute(Attr.Class, "material-icons"), this.IconClass);
            i.PostElement.SetContent(" " + this.Text);
            var b = TagHelper.Create(Tag.b, new TagAttribute(Attr.Class, "caret"));
            var a = TagHelper.Create(Tag.a, attributes, i, b);

            var h = TagHelper.ToHtml(a);
            return h;
        }
    }
}
