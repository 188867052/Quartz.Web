namespace Quartz.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using Quartz.Entity;
    using Quartz.Html.TextBoxs;
    using Quartz.Javascript;
    using Quartz.Models;
    using Quartz.ViewConfiguration;
    using Resource = Web.Resources.EditScheduleConfigurationResource;

    public class SchedualSearchFilterConfiguration<TModel, TPostModel>
        : SearchFilterConfigurationBase<TModel, TPostModel>
          where TPostModel : SchedulePostModel
            where TModel : TaskScheduleModel
    {
        protected override void CreateSearchFilter(IList<LargeColumn<TModel, TPostModel>> filters)
        {
            var name = new FloatTextBox<TModel, TPostModel>(Resource.Name, o => o.Name, o => o.Name);
            var group = new FloatTextBox<TModel, TPostModel>(Resource.Group, o => o.Group, o => o.Group);
            var url = new FloatTextBox<TModel, TPostModel>(Resource.Url, o => o.Url, o => o.Url);
            var cron = new FloatTextBox<TModel, TPostModel>("Cron", o => o.Cron, o => o.CronExpression);
            var startTime = new DateTimeFloatTextBox<TModel, TPostModel>("开始时间", o => o.StartTime, o => o.StartTime);
            var endTime = new DateTimeFloatTextBox<TModel, TPostModel>("结束时间", o => o.EndTime, o => o.EndTime);
            var lastExcuteTime = new DateTimeFloatTextBox<TModel, TPostModel>("上次执行时间", o => o.LastExcuteTime, o => o.PrevFireTime);
            var nextExcuteTime = new DateTimeFloatTextBox<TModel, TPostModel>("下次执行时间", o => o.NextExcuteTime, o => o.NextFireTime);
            var httpMethod = new SingleSelect<TModel, TPostModel, HttpMethod>(Resource.HttpMethod, o => o.HttpMethod, o => o.HttpMethod, o => (byte)o < 4 || (byte)o == 255)
            {
                Width = ComulnWidth.Two,
                Init = true,
            };
            filters.Add(new LargeColumn<TModel, TPostModel>(name) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(group) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(url) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(cron) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(startTime) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(endTime) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(lastExcuteTime) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(nextExcuteTime) { IsFilter = true });
            filters.Add(new LargeColumn<TModel, TPostModel>(httpMethod) { IsFilter = true });
        }
    }
}
