namespace MaterialUI.Html.GridColumn
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using AspNetCore.Extensions;
    using MaterialUI.Html.Buttons;
    using MaterialUI.Javascript;

    public class ActionGridColumn<T> : BaseGridColumn<T>
    {
        private readonly IList<ColumnButtonBase<T>> actionButtons;
        private readonly Expression<Func<T, int>> expression;

        public ActionGridColumn(string thead, Expression<Func<T, int>> expression)
            : base(thead)
        {
            this.actionButtons = new List<ColumnButtonBase<T>>();
            this.expression = expression;
        }

        public void AddModalButton(string buttonClass, Expression<Func<T, string>> iconClass, Expression<Func<T, string>> action, Identifier id)
        {
            this.actionButtons.Add(new ColumnModalButton<T>(buttonClass, iconClass, action, id));
        }

        public void AddActionButton(string buttonClass, Expression<Func<T, string>> iconClass, Expression<Func<T, string>> action)
        {
            this.actionButtons.Add(new ActionButton<T>(buttonClass, iconClass, action));
        }

        public override string RenderTh()
        {
            var name = this.expression.GetPropertyName();
            return $"<th  class=\"text-right\" name=\"{name}\">{this.Thead}</th>";
        }

        public override string RenderTd(T entity)
        {
            Check.NotNull(entity, nameof(entity));

            StringBuilder sb = new StringBuilder();
            foreach (var item in this.actionButtons)
            {
                var value = this.expression.Compile()(entity);
                sb.AppendLine(item.ToHtml(entity, value));
            }

            return "<td class=\"td-actions text-right\">" + sb.ToString() + "</td>";
        }
    }
}