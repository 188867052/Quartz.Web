namespace MaterialUI.ViewConfiguration.Schedule
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using Javascript;
    using MaterialUI.Entity;
    using MaterialUI.GridConfiguration;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.Dialog;
    using Microsoft.AspNetCore.Html;

    public class LogDialog : DialogBase
    {
        private readonly IList<QuartzLog> models;
        private readonly int index;
        private readonly int size;
        private readonly int total;
        private readonly string name;
        private readonly string group;

        public LogDialog(IList<QuartzLog> models, int index, int size, int total, string name, string group)
        {
            this.models = models;
            this.index = index;
            this.size = size;
            this.total = total;
            this.name = name;
            this.group = group;
        }

        protected override Identifier Identifier => ScheduleIdentifiers.LogDialogIdentifier;

        protected override string Title => $"Name: {this.name},Group:{this.group} -- 任务日志";

        protected override int Width { get; } = 80;

        protected IHtmlContent Grid
        {
            get
            {
                var grid = new LogDialogGridConfiguration<QuartzLog>(name, group);
                var responsiveTable = grid.Render(this.index, this.size, this.models, this.total);
                return responsiveTable;
            }
        }

        protected override IHtmlContent Body => TagHelper.Div("modal-body", this.Grid);

        protected override IHtmlContent Footer
        {
            get
            {
                var button = new CancleButton("关闭");
                var footer = TagHelper.Div("modal-footer", button.Render());
                return footer;
            }
        }
    }
}