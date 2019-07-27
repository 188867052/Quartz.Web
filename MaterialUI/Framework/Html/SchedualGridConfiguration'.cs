namespace MaterialUI.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using MaterialUI.Entity;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Routes;
    using MaterialUI.ViewConfiguration.Schedule;

    public class SchedualGridConfiguration<TModel>
        : GridConfigurationBase<TModel>
        where TModel : TaskScheduleModel
    {
        public override string GridStateChange
        {
            get
            {
                return ScheduleRoute.GridStateChange;
            }
        }

        public override object Data => new { };

        protected override void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns)
        {
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Name, "名称"));
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Group, "分组"));
            gridColumns.Add(new TextGridColumn<TModel>(o => o.TriggerState, "状态"));
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Url, "Url") { MaxLength = 40 });
            gridColumns.Add(new TextGridColumn<TModel>(o => o.CronExpression, "Schedual"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.StartTime, "开始时间"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.EndTime, "结束时间"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.PrevFireTime, "上次执行时间"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.NextFireTime, "下次执行时间"));

            ActionGridColumn<TModel> actionColumns = new ActionGridColumn<TModel>("Actions", o => new { o.Name, o.Group, index = 1, size = 10 });
            actionColumns.AddActionButton("btn btn-action btn-info btn-round", o => o.IconClass, o => o.IsPaused ? ScheduleRoute.ResumeJob : ScheduleRoute.StopJob);
            actionColumns.AddModalButton("btn btn-success btn-simple", o => "edit", o => ScheduleRoute.Edit, ScheduleIdentifiers.EditDialogIdentifier);
            actionColumns.AddModalButton("btn btn-danger", o => "close", o => ScheduleRoute.DeleteDialog, ScheduleIdentifiers.DeleteDialogIdentifier);
            actionColumns.AddModalButton("btn btn-info btn-round", o => "book", o => ScheduleRoute.LogDialog, ScheduleIdentifiers.LogDialogIdentifier);
            gridColumns.Add(actionColumns);
        }
    }
}
