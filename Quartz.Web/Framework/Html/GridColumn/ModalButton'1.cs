﻿namespace Quartz.Html.GridColumn
{
    using System;
    using System.Linq.Expressions;
    using AspNetCore.Extensions;
    using Newtonsoft.Json;
    using Quartz.Html.Icons;
    using Quartz.Html.Tags;

    public abstract class ColumnButtonBase<T>
    {
        private readonly string buttonClass;
        private readonly Expression<Func<T, string>> iconClass;
        private readonly Expression<Func<T, string>> action;

        public ColumnButtonBase(string buttonClass, Expression<Func<T, string>> iconClass, Expression<Func<T, string>> action)
        {
            this.buttonClass = buttonClass;
            this.iconClass = iconClass;
            this.action = action;
        }

        protected virtual TagAttributeList Attributes
        {
            get
            {
                return new TagAttributeList()
                {
                    new TagAttribute(Attr.Rel, "tooltip"),
                };
            }
        }

        public virtual string ToHtml(T entity, object data)
        {
            var icon = this.iconClass.Compile()(entity);
            var button = TagHelper.Create(Tag.button, this.Attributes, new MaterialIcon(icon).Html);
            var url = this.action.Compile()(entity);

            button.Attributes.Add(Attr.Type, "button");
            button.Attributes.Add(Attr.Class, this.buttonClass);
            button.Attributes.Add(Attr.Action, url);
            button.Attributes.Add(Attr.Data, JsonConvert.SerializeObject(data));
            button.PostElement.Append(" ");
            return TagHelper.ToHtml(button);
        }
    }
}
