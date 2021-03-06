﻿namespace Quartz.Html.Buttons
{
    using AspNetCore.Extensions;
    using Quartz.Html.Tags;
    using Quartz.Javascript;

    public class CancleButton : SimpleButton
    {
        public CancleButton(string labelText)
            : base(labelText)
        {
        }

        protected override JavaScriptEvent Event => default;

        protected override string ButtonClass => "btn btn-danger btn-simple";

        protected override TagAttributeList Attributes
        {
            get
            {
                return new TagAttributeList()
                {
                    new TagAttribute(Attr.Type, "button"),
                    new TagAttribute(Attr.Class, this.ButtonClass),
                    new TagAttribute(Attr.DataDismiss, "modal"),
                };
            }
        }
    }
}
