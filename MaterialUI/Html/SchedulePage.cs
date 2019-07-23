namespace MaterialKit.Controllers
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Dialog.Demo;
    using MaterialKit.Html.Tags;

    public class SchedulePage
    {
        public string ToHtml()
        {
            TagAttribute attribute = new TagAttribute(Attr.Class, "col-md-6 col-lg-4");
            var div = TagHelper.Create(Tag.div, attribute,
                new ScheduleCard("任务 1", "https://translate.google.cn").ToHtml());
            div.PostElement.AppendHtml(TagHelper.Create(Tag.div, attribute,
                new ScheduleCard("任务 2", "https://translate.google.cn").ToHtml()));
            div.PostElement.AppendHtml(TagHelper.Create(Tag.div, attribute,
                new ScheduleCard("任务 3", "https://translate.google.cn").ToHtml()));
            div.PostElement.AppendHtml(TagHelper.Create(Tag.div, attribute,
                new ScheduleCard("任务 4", "https://translate.google.cn").ToHtml()));

            return TagHelper.ToHtml(div);
        }
    }
}
