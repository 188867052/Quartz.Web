namespace MaterialUI.ViewConfiguration.Schedule
{
    using System.Collections.Generic;
    using Enums;
    using Html.TextBoxs;
    using Javascript;
    using MaterialUI.Entity;
    using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
    using Routes;
    using Resource = Resources.EditScheduleConfigurationResource;

    public class EditScheduleDialog<TModel, TPostModel> : EditDialogBase<TModel, TPostModel>
        where TPostModel : TaskScheduleModel
        where TModel : TaskScheduleModel
    {
        public EditScheduleDialog(TModel entity = null)
            : base(entity)
        {
        }

        protected override string Title => this.IsEdit ? Resource.EditTitle : Resource.AddTitle;

        protected override Identifier Identifier => ScheduleIdentifiers.EditDialogIdentifier;

        protected override void CreateColumns(IList<LargeColumn<TModel, TPostModel>> columns)
        {
            var name = new FloatTextBox<TModel, TPostModel>(Resource.Name, o => o.Name, o => o.Name);
            var group = new FloatTextBox<TModel, TPostModel>(Resource.Group, o => o.Group, o => o.Group);
            var url = new FloatTextBox<TModel, TPostModel>(Resource.Url, o => o.Url, o => o.Url);
            var startTime = new DateTimeFloatTextBox<TModel, TPostModel>(Resource.StartTime, o => o.StartTime, o => o.StartTime);
            var endTime = new DateTimeFloatTextBox<TModel, TPostModel>(Resource.EndTime, o => o.EndTime, o => o.EndTime);
            var httpMethod = new SingleSelect<TModel, TPostModel, HttpMethod>(Resource.HttpMethod, o => o.HttpMethod, o => o.HttpMethod, o => (byte)o < 4)
            {
                Width = ComulnWidth.Two,
                Init = true,
            };
            var triggerType = new SingleSelect<TModel, TPostModel, TriggerTypeEnum>(Resource.TriggerType, o => o.TriggerType, o => o.TriggerType)
            {
                Width = ComulnWidth.Four,
                Url = ScheduleRoute.ReplaceColumn,
                Function = "index.changeTriggerType",
                Id = Identifier.NewId,
                Data = new { this.Model?.Id },
                Init = true,
            };
            var isEnable = new CheckBox<TModel, TPostModel>(Resource.IsEnableLable, o => o.IsEnable, o => o.IsEnable);

            columns.Add(new LargeColumn<TModel, TPostModel>(name, group));
            columns.Add(new LargeColumn<TModel, TPostModel>(url));
            columns.Add(new LargeColumn<TModel, TPostModel>(startTime, endTime));
            columns.Add(new LargeColumn<TModel, TPostModel>(httpMethod, triggerType));
            ReplaceLargeColumn<TModel, TPostModel> replaceColumn = new ReplaceLargeColumn<TModel, TPostModel>(this.Model);
            replaceColumn.AddToColumns(columns);
            columns.Add(new LargeColumn<TModel, TPostModel>(isEnable));
        }

        protected override void CreateButtons(IList<ButtonColumn<TModel, TPostModel>> columns)
        {
            var button = new FooterButton<TModel, TPostModel>(ScheduleRoute.ModifyJob, o => o.Id, o => o.Id, Resource.OkLable);
            columns.Add(new ButtonColumn<TModel, TPostModel>(button));
        }
    }
}