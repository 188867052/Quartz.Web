namespace MaterialUI.Html.Inputs
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;

    public class SingleSelect
    {
        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "selectpicker"),
                new TagAttribute(Attr.DataStyle, "btn btn-primary btn-round"),
                new TagAttribute(Attr.Title, "Single Select"),
                new TagAttribute(Attr.DataSize, "7"),
            };

            TagAttributeList attributes2 = new TagAttributeList()
            {
                new TagAttribute(Attr.Disabled),
                new TagAttribute(Attr.Selected),
            };

            var option1 = TagHelper.Create(Tag.option, attributes2, "Choose city");
            var option2 = TagHelper.Create(Tag.option, new TagAttribute(Attr.Value, "2"), "Foobar");
            var option3 = TagHelper.Create(Tag.option, new TagAttribute(Attr.Value, "3"), "Is great");
            var select = TagHelper.Create(Tag.select, attributes, option1, option2, option3);

            return TagHelper.ToHtml(select);
        }
    }
}
