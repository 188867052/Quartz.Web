namespace Quartz.Html.Icons
{
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Quartz.Html.Tags;
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
