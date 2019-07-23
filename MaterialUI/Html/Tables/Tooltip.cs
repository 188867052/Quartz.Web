namespace MaterialKit.Html.Tables
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;

    public class Tooltip
    {
        private readonly string text;
        private readonly string placement;
        private readonly string title;

        public Tooltip(string text, string placement, string title)
        {
            this.text = text;
            this.placement = placement;
            this.title = title;
        }

        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Class, "btn btn-default btn-tooltip"),
                new TagAttribute(Attr.DataToggle, "tooltip"),
                new TagAttribute(Attr.DataPlacement, this.placement),
                new TagAttribute(Attr.Title, this.title),
                new TagAttribute(Attr.DataContainer, "body"),
            };
            var button = TagHelper.Create(Tag.button, attributes, this.text);

            return TagHelper.ToHtml(button);
        }

        public static string Generate()
        {
            var tooltip1 = new Tooltip("On left", "left", "Tooltip on left");
            var tooltip2 = new Tooltip("On top", "top", "Tooltip on top");
            var tooltip3 = new Tooltip("On bottom", "bottom", "Tooltip on bottom");
            var tooltip4 = new Tooltip("On right", "right", "Tooltip on right");
            return tooltip1.ToHtml() + tooltip2.ToHtml() + tooltip3.ToHtml() + tooltip4.ToHtml();
        }
    }
}
