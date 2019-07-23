namespace MaterialUI.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using MaterialUI.Entity;
    using MaterialUI.Html.GridColumn;

    public class LogDialogGridConfiguration<TModel>
      : GridConfigurationBase<TModel>
      where TModel : Log
    {
        protected override void CreateGridColumn(IList<BaseGridColumn<TModel>> gridColumns)
        {
            gridColumns.Add(new TextGridColumn<TModel>(o => o.Message, "Message"));
            gridColumns.Add(new EnumGridColumn<TModel>(o => o.LogLevel, "LogLevel"));
            gridColumns.Add(new DateTimeGridColumn<TModel>(o => o.CreateTime, "CreateTime"));
        }
    }
}
