namespace MaterialKit.Html.Icons
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using TagHelper = AspNetCore.Extensions.TagHelper;

    public class FontAwesomeIcon
    {
        public TagHelperOutput Html
        {
            get
            {
                return TagHelper.Create(Tag.i, new TagAttribute(Attr.Class, "fa fa-group"));
            }
        }
    }
}
