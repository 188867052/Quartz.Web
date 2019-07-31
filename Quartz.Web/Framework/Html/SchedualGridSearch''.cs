namespace Quartz.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Quartz.Controllers;
    using Quartz.Entity;
    using Quartz.Files.Js;
    using Quartz.Html;
    using Quartz.Html.Buttons;
    using Quartz.Html.Tags;
    using Quartz.Javascript;
    using Quartz.Models;
    using Quartz.Routes;
    using Quartz.ViewConfiguration.Schedule;
    using TagHelper = AspNetCore.Extensions.TagHelper;

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

        protected override IHtmlContent Header()
        {
            var header = (TagHelperOutput)base.Header();
            header.PostElement.AppendHtml(TagHelper.Create(Tag.meta, new TagAttributeList { { "http-equiv", "refresh" }, { Attr.Content, "60" }, }));
            return header;
        }

        protected override void CreateButtons(IList<SimpleButton> buttons)
        {
            buttons.Add(new ModalButton("添加", ScheduleRoute.AddDialog, ScheduleIdentifiers.EditDialogIdentifier));
            buttons.Add(new ScriptButton("搜索", "index.search", ScheduleRoute.Search));
            buttons.Add(new ScriptButton("刷新", "index.search", ScheduleRoute.Search));
        }

        protected override IList<string> JavaScriptFiles()
        {
            return new List<string>() { JsScheduleFile.IndexJs };
        }
    }
}
