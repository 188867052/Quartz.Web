namespace Quartz.Html.Inputs
{
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Quartz.Html.Tags;

    public class AnchorHref
    {
        private readonly string text;
        private readonly string href;

        public AnchorHref(string text, string href = "javascript:void(0);")
        {
            this.text = text;
            this.href = href;
        }

        public AnchorHref(int text)
            : this(text.ToString())
        {
        }

        public TagHelperOutput Html => AspNetCore.Extensions.TagHelper.Create(Tag.a, new TagAttribute(Attr.Href, this.href), this.text);
    }
}
