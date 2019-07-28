namespace Quartz.ViewConfiguration.Schedule
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;
    using Quartz.Entity;
    using Quartz.Enums;
    using Quartz.Html.TextBoxs;
    using Quartz.Javascript;
    using Resource = Web.Resources.EditScheduleConfigurationResource;

    public class ReplaceLargeColumn<TModel, TPostModel>
        where TPostModel : EdieTaskScheduleModel
        where TModel : EdieTaskScheduleModel
    {
        private readonly TModel model;
        private readonly EmptyColumn<TModel, TPostModel> emptyColumn;

        public ReplaceLargeColumn(TModel model = null)
        {
            this.model = model;
            this.emptyColumn = Startup.ServiceProvider.GetRequiredService<EmptyColumn<TModel, TPostModel>>();
        }

        public IHtmlContent Render()
        {
            var columns = new List<LargeColumn<TModel, TPostModel>>();
            this.AddToColumns(columns);
            IList<IHtmlContent> list = new List<IHtmlContent>();
            foreach (var item in columns)
            {
                list.Add(item.Render(this.model));
            }

            return TagHelper.Combine(list);
        }

        public void AddToColumns(IList<LargeColumn<TModel, TPostModel>> columns)
        {
            var cronExpression = new FloatTextBox<TModel, TPostModel>(Resource.CronExpression, o => o.CronExpression, o => o.CronExpression);
            var intervalTime = new IntegerFloatTextBox<TModel, TPostModel>(Resource.IntervalTime, o => o.IntervalTime, o => o.IntervalTime) { Width = ComulnWidth.Three };
            var intervalType = new SingleSelect<TModel, TPostModel, TimeSpanParseRule>(Resource.IntervalType, o => o.IntervalType, o => o.IntervalType) { Width = ComulnWidth.Three, Init = true, };

            if (this.model?.TriggerType == TriggerTypeEnum.Simple)
            {
                columns.Add(new LargeReplaceColumn<TModel, TPostModel>(intervalTime, intervalType));
            }
            else
            {
                columns.Add(new LargeReplaceColumn<TModel, TPostModel>(cronExpression, this.emptyColumn));
            }
        }
    }
}