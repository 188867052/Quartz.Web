namespace MaterialUI.Html.GridColumn
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;
    using MaterialUI.Javascript;

    public class ColumnModalButton<T> : ColumnButtonBase<T>
    {
        private readonly Identifier id;

        public ColumnModalButton(string buttonClass, Expression<Func<T, string>> iconClass, Expression<Func<T, string>> action, Identifier id)
            : base(buttonClass, iconClass, action)
        {
            this.id = id;
        }

        protected override TagAttributeList Attributes
        {
            get
            {
                return new TagAttributeList()
                {
                    new TagAttribute(Attr.Rel, "tooltip"),
                    new TagAttribute(Attr.DataToggle, "modal"),
                    new TagAttribute(Attr.DataTarget, $"#{this.id.Value}"),
                };
            }
        }
    }
}
