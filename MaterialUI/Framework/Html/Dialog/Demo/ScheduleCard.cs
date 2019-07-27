namespace Quartz.Html.Dialog.Demo
{
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Quartz.Html.Icons;
    using Quartz.Html.Tags;
    using TagHelper = AspNetCore.Extensions.TagHelper;

    public class ScheduleCard
    {
        private readonly string url;
        private readonly string name;

        public string Status { get; set; }

        public string Cron { get; set; }

        public ScheduleCard(string name, string url)
        {
            this.name = name;
            this.url = url;
        }

        public string ToHtml()
        {
            var div2 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "card card-rotate"), this.FrontToHtml(), this.BackToHtml());
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "rotating-card-container manual-flip"), div2);

            return TagHelper.ToHtml(div);
        }

        public TagHelperOutput FrontToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "front front-background"),
                new TagAttribute(Attr.Style, "background-image: url('/img/examples/card-blog4.jpg');"),
            };

            TagAttributeList attributes1 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Name, "button"),
                new TagAttribute(Attr.Class, "btn btn-danger btn-fill btn-round btn-rotate"),
            };

            var button = TagHelper.Create(Tag.button, attributes1, new MaterialIcon("refresh", " Rotate...").Html);
            var footer = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "footer text-center"), button);
            var h3 = TagHelper.Create(Tag.h3, new TagAttribute(Attr.Class, "card-title"), "This card is doing a full rotation, click on the rotate button");
            var a = TagHelper.Create(Tag.a, new TagAttribute(Attr.Href, "#pablo"), h3);
            var h6 = TagHelper.Create(Tag.h6, new TagAttribute(Attr.Class, "category text-info"), this.name);
            var p = TagHelper.Create(Tag.p, new TagAttribute(Attr.Class, "card-description"), "Don't be scared of the truth because we need to restart the human...");

            var cardcontent = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "card-content"), h6, a, p, footer);
            var front = TagHelper.Create(Tag.div, attributes, cardcontent);
            return front;
        }

        public TagHelperOutput BackToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "back back-background"),
                new TagAttribute(Attr.Style, "background-image: url('/img/examples/card-blog6.jpg');"),
            };

            TagAttributeList attributes2 = new TagAttributeList()
            {
                new TagAttribute(Attr.Href, "#pablo"),
                new TagAttribute(Attr.Class, "btn btn-info btn-just-icon btn-fill btn-round"),
            };

            TagAttributeList attributes3 = new TagAttributeList()
            {
                new TagAttribute(Attr.Href, "#pablo"),
                new TagAttribute(Attr.Class, "btn btn-success btn-just-icon btn-fill btn-round btn-wd"),
            };

            TagAttributeList attributes4 = new TagAttributeList()
            {
                new TagAttribute(Attr.Href, "#pablo"),
                new TagAttribute(Attr.Class, "btn btn-danger btn-just-icon btn-fill btn-round"),
            };

            var a1 = TagHelper.Create(Tag.a, attributes2, new MaterialIcon("directions_run").Html);
            var a2 = TagHelper.Create(Tag.a, attributes3, new MaterialIcon("mode_edit").Html);
            var a3 = TagHelper.Create(Tag.a, attributes4, new MaterialIcon("delete").Html);
            var footer = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "footer text-center"), a1, a2, a3);
            TagAttributeList attributes1 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Name, "button"),
                new TagAttribute(Attr.Class, "btn btn-success btn-fill btn-round btn-rotate"),
            };
            var button = TagHelper.Create(Tag.button, attributes1, new MaterialIcon("refresh", " Back...").Html);
            var footer2 = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "footer text-center"), button);
            var br = TagHelper.StartOnlyTag(Tag.br);
            var h5 = TagHelper.Create(Tag.h5, new TagAttribute(Attr.Class, "card-title"), "Manage Post");
            var p = TagHelper.Create(Tag.p, new TagAttribute(Attr.Class, "card-description"), this.url);
            var cardcontent = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "card-content"), h5, p, footer, br, footer2);
            var back = TagHelper.Create(Tag.div, attributes, cardcontent);
            return back;
        }
    }
}
