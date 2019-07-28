namespace Quartz.Controllers
{
    using Quartz.Javascript;

    public class SchedualViewInstance : ViewInstanceConstruction
    {
        protected override string InstanceClassName => "Index";

        public override void InitializeViewInstance(JavaScriptInitialize initialize)
        {
        }
    }
}
