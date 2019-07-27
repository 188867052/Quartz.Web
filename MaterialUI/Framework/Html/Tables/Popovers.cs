namespace Quartz.Html.Tables
{
    using AspNetCore.Extensions;
    using Quartz.Html.Tags;

    public class Popovers
    {
        private readonly string text;
        private readonly string placement;
        private readonly string title;
        private readonly string content;

        public Popovers(string text, string placement, string title, string content)
        {
            this.text = text;
            this.placement = placement;
            this.title = title;
            this.content = content;
        }

        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Class, "btn btn-default"),
                new TagAttribute(Attr.DataToggle, "popover"),
                new TagAttribute(Attr.DataPlacement, this.placement),
                new TagAttribute(Attr.Title, this.title),
                new TagAttribute(Attr.DataContent, this.content),
                new TagAttribute(Attr.DataContainer, "body"),
            };
            var button = TagHelper.Create(Tag.button, attributes, this.text);

            return TagHelper.ToHtml(button);
        }

        public static string Generate()
        {
            var popovers1 = new Popovers("On left", "left", "Popovers on left", "Here will be some very useful information about his popover. Here will be some very useful information about his popover.");
            var popovers2 = new Popovers("On top", "top", "Popovers on top", "Here will be some very useful information about his popover.");
            var popovers3 = new Popovers("On bottom", "bottom", "Popovers on bottom", "Here will be some very useful information about his popover.");
            var popovers4 = new Popovers("On right", "right", "Popovers on right", "Here will be some very useful information about his popover.");
            return popovers1.ToHtml() + popovers2.ToHtml() + popovers3.ToHtml() + popovers4.ToHtml();
        }
    }
}
