namespace MaterialKit.ViewConfiguration.Schedule
{
    using MaterialKit.Javascript;

    public class DeleteDialogViewInstance : ViewInstanceConstruction
    {
        protected override string InstanceClassName => "Index";

        public override void InitializeViewInstance(JavaScriptInitialize initialize)
        {
            initialize.AddFrameWorkInstance("dialogInstance", ScheduleIdentifiers.DeleteDialogIdentifier);
        }
    }
}
