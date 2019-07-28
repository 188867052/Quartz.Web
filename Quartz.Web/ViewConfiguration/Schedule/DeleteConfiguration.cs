namespace Quartz.ViewConfiguration.Schedule
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Entity;
    using Quartz.Html.Buttons;
    using Quartz.Html.Dialog;
    using Quartz.Html.Tags;
    using Quartz.Javascript;
    using Quartz.Routes;

    public class DeleteConfiguration : DialogBase
    {
        private QuartzTriggers trigger;

        public DeleteConfiguration(QuartzTriggers trigger)
        {
            this.trigger = trigger;
        }

        protected override string Title => "删除";

        protected override int Width { get; } = 20;

        protected override Identifier Identifier => ScheduleIdentifiers.DeleteDialogIdentifier;

        protected override IHtmlContent Body
        {
            get
            {
                var p = TagHelper.Create(Tag.p, "确定删除吗?");
                var body = TagHelper.Div("modal-body text-center", p);
                return body;
            }
        }

        protected override IHtmlContent Footer
        {
            get
            {
                IList<SimpleButton> buttons = new List<SimpleButton>();
                buttons.Add(new CancleButton("取消"));
                buttons.Add(new SubmitButton("确定", "index.delete", ScheduleRoute.Delete, new { this.trigger.TriggerName, this.trigger.TriggerGroup }));

                IList<IHtmlContent> list = new List<IHtmlContent>();
                foreach (var item in buttons)
                {
                    list.Add(item.Render());
                }

                var footer = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-footer text-center"), list);
                return footer;
            }
        }
    }
}