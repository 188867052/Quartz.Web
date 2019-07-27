namespace MaterialUI.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using MaterialUI.Controllers;
    using MaterialUI.Entity;
    using MaterialUI.Files.Js;
    using MaterialUI.Html;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Javascript;
    using MaterialUI.Models;
    using MaterialUI.Routes;
    using MaterialUI.ViewConfiguration.Schedule;

    public class SchedualGridSearch<TModel, TPostModel> : GridSearchPage<TModel, TPostModel>
         where TPostModel : SchedulePostModel
         where TModel : TaskScheduleModel
    {
        public SchedualGridSearch(IList<TModel> list, int index, int size, int total)
            : base(list, index, size, total)
        {
        }

        protected override GridConfigurationBase<TModel> GridConfiguration => new SchedualGridConfiguration<TModel>();

        protected override SearchFilterConfigurationBase<TModel, TPostModel> SearchFilterConfiguration => new SchedualSearchFilterConfiguration<TModel, TPostModel>();

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
            buttons.Add(new ScriptButton("刷新", "index.search", ScheduleRoute.Search));
        }

        protected override IList<string> JavaScriptFiles()
        {
            return new List<string>() { JsScheduleFile.IndexJs };
        }
    }
}
