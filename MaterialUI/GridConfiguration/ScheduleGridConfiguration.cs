namespace MaterialUI.GridConfigurations.Schedule
{
    using System.Collections.Generic;
    using MaterialUI.Entity;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Routes;
    using MaterialUI.ViewConfiguration.Schedule;

    public class ScheduleGridConfiguration<T> : GridConfiguration<T>
        where T : TaskScheduleModel
    {
        public ScheduleGridConfiguration(IList<T> model)
            : base(model)
        {
        }

        protected override void CreateButtons(IList<SimpleButton> buttons)
        {
            buttons.Add(new ModalButton("添加", ScheduleRoute.Add, ScheduleIdentifiers.EditDialogIdentifier));
            buttons.Add(new ScriptButton("搜索", "index.search", ScheduleRoute.Search));
        }

        protected override void CreateGridColumn(IList<BaseGridColumn<T>> gridColumns)
        {
            gridColumns.Add(new TextGridColumn<T>(o => o.Name, "名称"));
            gridColumns.Add(new TextGridColumn<T>(o => o.Group, "分组"));
            gridColumns.Add(new TextGridColumn<T>(o => o.Status.ToString(), "状态"));
            gridColumns.Add(new TextGridColumn<T>(o => o.Url, "Url") { MaxLength = 40 });
            gridColumns.Add(new TextGridColumn<T>(o => o.CronExpression, "Cron"));
            gridColumns.Add(new DateTimeGridColumn<T>(o => o.StartTime, "开始时间"));
            gridColumns.Add(new DateTimeGridColumn<T>(o => o.EndTime, "结束时间"));
            gridColumns.Add(new DateTimeGridColumn<T>(o => o.PrevFireTime, "上次执行时间"));
            gridColumns.Add(new DateTimeGridColumn<T>(o => o.NextFireTime, "下次执行时间"));

            ActionGridColumn<T> actionColumns = new ActionGridColumn<T>("操作", o => o.Id);
            actionColumns.AddActionButton("btn btn-action btn-info btn-round", o => o.IconClass, o => o.IsEnable ? ScheduleRoute.StopJob : ScheduleRoute.ResumeJob);
            actionColumns.AddModalButton("btn btn-success btn-simple", o => "edit", o => ScheduleRoute.Edit, ScheduleIdentifiers.EditDialogIdentifier);
            actionColumns.AddModalButton("btn btn-danger", o => "close", o => ScheduleRoute.DeleteDialog, ScheduleIdentifiers.DeleteDialogIdentifier);
            actionColumns.AddModalButton("btn btn-info btn-round", o => "book", o => ScheduleRoute.GetJobLogs, ScheduleIdentifiers.LogDialogIdentifier);
            gridColumns.Add(actionColumns);
        }
    }
}
