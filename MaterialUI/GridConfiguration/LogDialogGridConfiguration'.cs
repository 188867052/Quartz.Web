namespace MaterialUI.GridConfiguration
{
    using System.Collections.Generic;
    using MaterialUI.Entity;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.Routes;
    using MaterialUI.SearchFilterConfigurations;

    public class LogDialogGridConfiguration<TModel>
      : GridConfigurationBase<TModel>
      where TModel : QuartzLog
    {
        private readonly string name;
        private readonly string group;

        public LogDialogGridConfiguration(string name, string group)
        {
            this.name = name;
            this.group = group;
        }
        protected override void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns)
        {
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Message, "Message") { MaxLength = 20 });
            gridColumns.Add(new EnumGridColumn<TModel>(o => o.LogLevel, "LogLevel"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.CreateTime, "CreateTime"));
        }

        public override string GridStateChange
        {
            get
            {
                return ScheduleRoute.LogGridStateChange;
            }
        }

        public override object Data => new { name, group };
    }
}
