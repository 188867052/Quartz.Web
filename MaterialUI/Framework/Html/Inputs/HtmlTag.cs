namespace MaterialUI.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;

    public class HtmlTag
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "text"),
                new TagAttribute(Attr.Value, "Amsterdam,Washington,Sydney,Beijing"),
                new TagAttribute(Attr.Class, "tagsinput"),
                new TagAttribute(Attr.DataRole, "tagsinput"),
                new TagAttribute(Attr.DataColor, "rose"),
                //<!-- You can change data-color="rose" with one of our colors primary | warning | info | danger | success -->
            };

            var div = TagHelper.SelfClosingTag(Tag.input, attributes);
            return TagHelper.ToHtml(div);
        }
    }
}
