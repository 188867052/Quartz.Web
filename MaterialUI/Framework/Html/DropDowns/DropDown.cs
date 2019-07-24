namespace MaterialUI.Html.DropDowns
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;

    public class DropDown
    {
        public DropDownToggle Toggle { get; set; }

        public IList<DropDownMenu> Menu { get; set; }

        public string ToHtml()
        {
            string htmlMenu = "";
            foreach (var item in this.Menu)
            {
                htmlMenu += item.ToHtml();
            }

            var ul = TagHelper.Create(Tag.ul, new TagAttribute(Attr.Class, "dropdown-menu dropdown-with-icons"), htmlMenu);
            var li = TagHelper.Create(Tag.li, new TagAttribute(Attr.Class, "dropdown"), this.Toggle.ToHtml() + TagHelper.ToHtml(ul));

            var h = TagHelper.ToHtml(li);
            return h;
        }
    }
}
