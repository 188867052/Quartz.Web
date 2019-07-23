namespace MaterialKit.Html.Dialog
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using MaterialKit.Html.Buttons;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;
    using MaterialKit.Javascript;
    using Microsoft.AspNetCore.Html;

    public abstract class ConfirmDialogConfiguration : IDialogConfiguration
    {
        protected ConfirmDialogConfiguration(Identifier identifier)
        {
            Check.NotNull(identifier, nameof(identifier));

            this.Identifier = identifier;
        }

        protected abstract string Text { get; }

        private Identifier Identifier { get; }

        private string Buttons
        {
            get
            {
                IList<SimpleButton> buttons = new List<SimpleButton>();
                this.CreateButtons(buttons);
                return TagHelper.ToHtml(buttons.Select(o => o.Render()));
            }
        }

        public virtual IHtmlContent Render()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Class, "close"),
                new TagAttribute(Attr.DataDismiss, "modal"),
                new TagAttribute(Attr.AriaHidden, "true"),
            };
            var button = TagHelper.Create(Tag.button, attributes, new MaterialIcon("clear").Html);
            var header = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-header"), button);

            var h5 = TagHelper.Create(Tag.h5, this.Text);
            var body = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-body text-center"), h5);
            var footer = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-footer text-center"), this.Buttons);
            var content = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-content"), header, body, footer);
            var dialog = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-dialog modal-small"), content);

            TagAttributeList attributes1 = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "modal fade"),
                new TagAttribute(Attr.Id, this.Identifier.Value),
                new TagAttribute(Attr.TabIndex, "-1"),
                new TagAttribute(Attr.Role, "dialog"),
                new TagAttribute(Attr.AriaLabelledBy, "myModalLabel"),
                new TagAttribute(Attr.AriaHidden, "true"),
            };

            return TagHelper.Create(Tag.div, attributes1, dialog);
        }

        protected abstract void CreateButtons(IList<SimpleButton> buttons);
    }
}
