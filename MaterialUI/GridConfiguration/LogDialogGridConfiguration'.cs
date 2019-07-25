namespace MaterialUI.GridConfiguration
{
    using System.Collections.Generic;
    using MaterialUI.Entity;
    using MaterialUI.Html.GridColumn;
    using MaterialUI.SearchFilterConfigurations;

    public class LogDialogGridConfiguration<TModel>
      : GridConfigurationBase<TModel>
      where TModel : QuartzLog
    {
        protected override void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns)
        {
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Message, "Message") { MaxLength = 20 });
            gridColumns.Add(new EnumGridColumn<TModel>(o => o.LogLevel, "LogLevel"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.CreateTime, "CreateTime"));
        }
    }
}
