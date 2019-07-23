namespace MaterialUI.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;

    public class Pagination
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "active"),
            };

            var li = TagHelper.Create(Tag.li, new AnchorHref("...").Html);
            var li1 = TagHelper.Create(Tag.li, new AnchorHref(1).Html);
            var li5 = TagHelper.Create(Tag.li, new AnchorHref(5).Html);
            var li6 = TagHelper.Create(Tag.li, new AnchorHref(6).Html);
            var li7 = TagHelper.Create(Tag.li, attributes, new AnchorHref(7).Html);
            var li8 = TagHelper.Create(Tag.li, new AnchorHref(8).Html);
            var li9 = TagHelper.Create(Tag.li, new AnchorHref(9).Html);
            var li12 = TagHelper.Create(Tag.li, new AnchorHref(12).Html);
            var ul2 = TagHelper.Create(Tag.ul, new TagAttribute(Attr.Class, "pagination pagination-info"),
                li1, li, li5, li6, li7, li8, li9, li, li12);

            return TagHelper.ToHtml(ul2);
        }
    }
}
