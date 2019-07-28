namespace Quartz.Html.DropDowns
{
    using AspNetCore.Extensions;
    using Quartz.Html.Tags;

    public class DropDownMenu
    {
        public DropDownMenu(string href, string iconClass, string text)
        {
            this.Href = href;
            this.IconClass = iconClass;
            this.Text = text;
        }

        public string Href { get; set; }

        public string IconClass { get; set; }

        public string Text { get; set; }

        public string ToHtml()
        {
            var i = TagHelper.Create(Tag.i, new TagAttribute(Attr.Class, "material-icons"), this.IconClass);
            i.PostElement.SetContent(" " + this.Text);
            var a = TagHelper.Create(Tag.a, new TagAttribute(Attr.Href, this.Href), i);
            var li = TagHelper.Create(Tag.li, a);

            var h = TagHelper.ToHtml(li);
            return h;
        }
    }
}
