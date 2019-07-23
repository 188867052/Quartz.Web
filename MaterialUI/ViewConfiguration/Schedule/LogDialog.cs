namespace MaterialUI.ViewConfiguration.Schedule
{
    using System.Linq;
    using AspNetCore.Extensions;
    using Javascript;
    using MaterialUI.Dapper;
    using MaterialUI.Entity;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.Dialog;
    using MaterialUI.SearchFilterConfigurations;
    using Microsoft.AspNetCore.Html;

    public class LogDialog : DialogBase
    {
        public override Identifier Identifier => ScheduleIdentifiers.LogDialogIdentifier;

        public override string Title => "日志";

        public override int Width { get; } = 80;

        public IHtmlContent Grid
        {
            get
            {
                var grid = new LogDialogGridConfiguration<Log>();
                var list = DapperExtension.Page<Log>(1, 10, out int count).OrderByDescending(o => o.CreateTime).Select().ToList();
                var responsiveTable = grid.Render(1, 10, list, count);
                return responsiveTable;
            }
        }

        public override IHtmlContent Body
        {
            get
            {
                return TagHelper.Div("modal-body", this.Grid);
            }
        }

        public override IHtmlContent Footer
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