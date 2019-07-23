namespace MaterialUI.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using MaterialKit.Controllers;
    using MaterialKit.Entity;
    using MaterialKit.Files.Js;
    using MaterialKit.Html;
    using MaterialKit.Html.Buttons;
    using MaterialKit.Html.GridColumn;
    using MaterialKit.Html.TextBoxs;
    using MaterialKit.Javascript;
    using MaterialKit.Models;
    using MaterialKit.Routes;
    using MaterialKit.ViewConfiguration;
    using MaterialKit.ViewConfiguration.Schedule;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using Resource = MaterialUI.Resources.EditScheduleConfigurationResource;

    public class SchedualGridSearch<TModel, TPostModel> : GridSearchPage<TModel, TPostModel>
         where TPostModel : SchedulePostModel
         where TModel : TaskSchedule
    {
        public SchedualGridSearch(IList<TModel> list)
            : base(list)
        {
        }

        protected override IList<string> CssFiles()
        {
            return new List<string>();
        }

        protected override IList<ViewInstanceConstruction> CreateViewInstanceConstructions()
        {
            return new List<ViewInstanceConstruction>
            {
                new SchedualViewInstance(),
            };
        }

        protected override void CreateButtons(IList<SimpleButton> buttons)
        {
            buttons.Add(new ModalButton("添加", ScheduleRoute.Add, ScheduleIdentifiers.EditDialogIdentifier));
            buttons.Add(new ScriptButton("搜索", "index.search", ScheduleRoute.Search));
        }

        protected override void Filter(IList<LargeColumn<TModel, TPostModel>> columns)
        {
            var name = new FloatTextBox<TModel, TPostModel>(Resource.Name, o => o.Name, o => o.Name);
            var group = new FloatTextBox<TModel, TPostModel>(Resource.Group, o => o.Group, o => o.Group);
            var url = new FloatTextBox<TModel, TPostModel>(Resource.Url, o => o.Url, o => o.Url);
            var cron = new FloatTextBox<TModel, TPostModel>("Cron", o => o.Cron, o => o.CronExpression);
            var startTime = new DateTimeFloatTextBox<TModel, TPostModel>("开始时间", o => o.StartTime, o => o.StartTime);
            var endTime = new DateTimeFloatTextBox<TModel, TPostModel>("结束时间", o => o.EndTime, o => o.EndTime);
            var lastExcuteTime = new DateTimeFloatTextBox<TModel, TPostModel>("上次执行时间", o => o.LastExcuteTime, o => o.LastExcuteTime);
            var nextExcuteTime = new DateTimeFloatTextBox<TModel, TPostModel>("下次执行时间", o => o.NextExcuteTime, o => o.NextExcuteTime);
            var httpMethod = new SingleSelect<TModel, TPostModel, HttpMethod>(Resource.HttpMethod, o => o.HttpMethod, o => o.HttpMethod, o => (byte)o < 4 || (byte)o == 255)
            {
                Width = ComulnWidth.Two,
                Init = true,
            };
            columns.Add(new LargeColumn<TModel, TPostModel>(name) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(group) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(url) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(cron) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(startTime) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(endTime) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(lastExcuteTime) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(nextExcuteTime) { IsFilter = true });
            columns.Add(new LargeColumn<TModel, TPostModel>(httpMethod) { IsFilter = true });
        }

        protected override void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns)
        {
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Name, "名称"));
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Group, "分组"));
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Status.ToString(), "状态"));
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Url, "Url") { MaxLength = 40 });
            gridColumns.Add(new TextGridColumn<TModel>(o => o.CronExpression, "Cron"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.StartTime, "开始时间"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.EndTime, "结束时间"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.LastExcuteTime, "上次执行时间"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.NextExcuteTime, "下次执行时间"));

            ActionGridColumn<TModel> actionColumns = new ActionGridColumn<TModel>("Actions", o => o.Id);
            actionColumns.AddActionButton("btn btn-action btn-info btn-round", o => o.IconClass, o => o.IsEnable ? ScheduleRoute.StopJob : ScheduleRoute.ResumeJob);
            actionColumns.AddModalButton("btn btn-success btn-simple", o => "edit", o => ScheduleRoute.Edit, ScheduleIdentifiers.EditDialogIdentifier);
            actionColumns.AddModalButton("btn btn-danger", o => "close", o => ScheduleRoute.DeleteDialog, ScheduleIdentifiers.DeleteDialogIdentifier);
            actionColumns.AddModalButton("btn btn-info btn-round", o => "book", o => ScheduleRoute.GetJobLogs, ScheduleIdentifiers.LogDialogIdentifier);
            gridColumns.Add(actionColumns);
        }

        protected override IList<string> JavaScriptFiles()
        {
            return new List<string>() { JsScheduleFile.IndexJs };
        }
    }
}
