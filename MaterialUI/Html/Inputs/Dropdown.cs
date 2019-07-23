namespace MaterialKit.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;

    public class Dropdown
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Href, "#pablo"),
                new TagAttribute(Attr.Class, "dropdown-toggle btn btn-primary btn-round btn-block"),
                new TagAttribute(Attr.DataToggle, "dropdown"),
            };
            var divider = TagHelper.Create(Tag.li, new TagAttribute(Attr.Class, "divider"));
            var header = TagHelper.Create(Tag.li, new TagAttribute(Attr.Class, "dropdown-header"), "Dropdown header");
            var li1 = TagHelper.Create(Tag.li, new AnchorHref("Action", "#pablo").Html);
            var li2 = TagHelper.Create(Tag.li, new AnchorHref("Another action", "#pablo").Html);
            var li3 = TagHelper.Create(Tag.li, new AnchorHref("Something else here", "#pablo").Html);
            var li4 = TagHelper.Create(Tag.li, new AnchorHref("Separated link", "#pablo").Html);
            var li5 = TagHelper.Create(Tag.li, new AnchorHref("One more separated link", "#pablo").Html);
            var ul = TagHelper.Create(Tag.ul, new TagAttribute(Attr.Class, "dropdown-menu dropdown-menu-right"),
                header, li1, li2, li3, divider, li4, divider, li5);
            var button = TagHelper.Create(Tag.button, attributes, "Dropdown ");
            var b = TagHelper.Create(Tag.b, new TagAttribute(Attr.Class, "caret"));
            button.Content.AppendHtml(b);
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "dropdown"), button, ul);
            var div2 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "dropup"), button, ul);
            return TagHelper.ToHtml(div, div2);
        }
    }
}
