namespace MaterialKit.Html.Buttons
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using MaterialKit.Html.GridColumn;

    public class ActionButton<T> : ColumnButtonBase<T>
    {
        public ActionButton(string buttonClass, Expression<Func<T, string>> iconClass, Expression<Func<T, string>> action)
            : base(buttonClass, iconClass, action)
        {
        }

        protected override TagAttributeList Attributes
        {
            get
            {
                var attributes = base.Attributes;
                return attributes;
            }
        }
    }
}