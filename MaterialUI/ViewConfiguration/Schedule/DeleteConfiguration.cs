namespace MaterialUI.ViewConfiguration.Schedule
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.Dialog;
    using MaterialUI.Html.Tags;
    using MaterialUI.Javascript;
    using MaterialUI.Routes;
    using Microsoft.AspNetCore.Html;

    public class DeleteConfiguration : DialogBase
    {
        private readonly int id;

        public DeleteConfiguration(int id)
        {
            this.id = id;
        }

        public override string Title => "删除";

        public override int Width { get; } = 20;

        public override Identifier Identifier => ScheduleIdentifiers.DeleteDialogIdentifier;

        public override IHtmlContent Body
        {
            get
            {
                var p = TagHelper.Create(Tag.p, "确定删除吗?");
                var body = TagHelper.Div("modal-body text-center", p);
                return body;
            }
        }

        public override IHtmlContent Footer
        {
            get
            {
                IList<SimpleButton> buttons = new List<SimpleButton>();
                buttons.Add(new CancleButton("取消"));
                buttons.Add(new SubmitButton("确定", "index.delete", ScheduleRoute.Delete, new { this.id }));

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