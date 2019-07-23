namespace MaterialKit.ViewConfiguration.Schedule
{
    using AspNetCore.Extensions;
    using Javascript;
    using MaterialKit.Html.Buttons;
    using MaterialKit.Html.Dialog;
    using MaterialKit.Html.Tags;
    using Microsoft.AspNetCore.Html;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class LogDialog : DialogBase
    {
        private readonly List<string> obj;

        public LogDialog(List<string> obj)
        {
            this.obj = obj;
        }

        public override Identifier Identifier => ScheduleIdentifiers.LogDialogIdentifier;

        public override string Title => "日志";

        public override int Width { get; } = 80;

        public override IHtmlContent Body
        {
            get
            {
                string log = "";
                if (this.obj != null)
                {
                    this.obj.Reverse();
                    log = JsonConvert.SerializeObject(this.obj.Take(10), Formatting.Indented);
                }

                var p = TagHelper.Create(Tag.p, log);
                var body = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-body"), p);
                return body;
            }
        }

        public override IHtmlContent Footer
        {
            get
            {
                var button = new CancleButton("关闭");
                var footer = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-footer"), button.Render());
                return footer;
            }
        }
    }
}