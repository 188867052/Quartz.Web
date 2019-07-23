namespace MaterialKit.Html.Inputs
{
    using System.Collections.Generic;
    using System.Text;
    using AspNetCore.Extensions;
    using MaterialKit.Entity;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;

    public class HtmlIcons
    {
        private readonly string @class;
        private readonly string display;

        public HtmlIcons(string @class, string display)
        {
            this.@class = @class;
            this.display = display;
        }

        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.DataToggle, "tooltip"),
                new TagAttribute(Attr.DataPlacement, "left"),
                new TagAttribute(Attr.DataContainer, "body"),
                new TagAttribute(Attr.DataOriginalTitle, this.@class),
            };

            var a = TagHelper.Create(Tag.a, attributes, new MaterialIcon(this.@class, this.display).Html);
            return TagHelper.ToHtml(a);
        }

        public static string Generate(List<Icon> icons)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in icons)
            {
                string display = item.Name;
                if (item.Name.Trim().Length > 10)
                {
                    display = item.Name.Substring(0, 7) + "...";
                }

                var icon = new HtmlIcons(item.Name.Trim(), display);
                var li = TagHelper.Create(Tag.li, icon.ToHtml());
                sb.Append(TagHelper.ToHtml(li));
            }

            return sb.ToString();
        }
    }
}
