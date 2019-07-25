namespace MaterialUI.ViewConfiguration.Schedule
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using Javascript;
    using MaterialUI.Dapper;
    using MaterialUI.Entity;
    using MaterialUI.GridConfiguration;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.Dialog;
    using Microsoft.AspNetCore.Html;

    public class LogDialog : DialogBase
    {
        private readonly List<QuartzLog> models;

        public LogDialog(List<QuartzLog> models)
        {
            this.models = models;
        }

        protected override Identifier Identifier => ScheduleIdentifiers.LogDialogIdentifier;

        protected override string Title => "日志";

        protected override int Width { get; } = 80;

        protected IHtmlContent Grid
        {
            get
            {
                var grid = new LogDialogGridConfiguration<QuartzLog>();
                var responsiveTable = grid.Render(1, 10, this.models.Take(10).ToList(), this.models.Count);
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