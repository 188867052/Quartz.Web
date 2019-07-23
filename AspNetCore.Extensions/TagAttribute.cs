namespace AspNetCore.Extensions
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class TagAttribute : TagHelperAttribute
    {
        public TagAttribute(string name) : base(name)
        {
        }

        public TagAttribute(string name, object value) : base(name, value)
        {
        }

        public TagAttribute(string name, object value, HtmlAttributeValueStyle valueStyle) : base(name, value, valueStyle)
        {
        }
    }

    public class TagAttributeList : TagHelperAttributeList
    {
    }
}
