namespace MaterialKit.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;

    public class MultipleSelect
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "selectpicker"),
                new TagAttribute(Attr.DataStyle, "select-with-transition"),
                new TagAttribute(Attr.Multiple),
                new TagAttribute(Attr.Title, "Choose City"),
                new TagAttribute(Attr.DataSize, "5"),
            };

            TagAttributeList attributes2 = new TagAttributeList()
            {
                new TagAttribute(Attr.Disabled),
            };

            var option1 = TagHelper.Create(Tag.option, attributes2, "Choose city");
            var option2 = TagHelper.Create(Tag.option, new TagAttribute(Attr.Value, "2"), "Foobar");
            var option3 = TagHelper.Create(Tag.option, new TagAttribute(Attr.Value, "3"), "Is great");
            var select = TagHelper.Create(Tag.select, attributes, option1, option2, option3);

            return TagHelper.ToHtml(select);
        }
    }
}
