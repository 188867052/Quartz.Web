﻿namespace Quartz.Html
{
    using Quartz.Javascript;

    public class FrameworkViewInstance : ViewInstanceConstruction
    {
        protected override string InstanceClassName => "Framework";

        public override void InitializeViewInstance(JavaScriptInitialize initialize)
        {
        }
    }
}
