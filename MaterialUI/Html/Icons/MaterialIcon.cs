namespace MaterialUI.Html.Icons
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using TagHelper = AspNetCore.Extensions.TagHelper;

    public class MaterialIcon
    {
        private readonly string text;
        private readonly string postContent;

        public MaterialIcon(string text, string postContent = default)
        {
            this.text = text;
            this.postContent = postContent;
        }

        public TagHelperOutput Html
        {
            get
            {
                var i = TagHelper.Create(Tag.i, new TagAttribute(Attr.Class, "material-icons"), this.text);
                if (!string.IsNullOrEmpty(this.postContent))
                {
                    i.PostElement.Append(this.postContent);
                }

                return i;
            }
        }
    }
}
