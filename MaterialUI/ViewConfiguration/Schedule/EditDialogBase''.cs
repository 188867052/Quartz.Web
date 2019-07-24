namespace MaterialUI.ViewConfiguration.Schedule
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using MaterialUI.Html.Dialog;
    using MaterialUI.Html.Icons;
    using MaterialUI.Html.Tags;
    using MaterialUI.Shared;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public abstract class EditDialogBase<TModel, TPostModel> : DialogBase
    {
        public EditDialogBase(TModel model)
        {
            this.Model = model;
        }

        public override IHtmlContent Render()
        {
            Check.NotEmpty(this.Container.ToList(), nameof(this.Container));

            var tag = (TagHelperOutput)this.Container.Last();
            tag.Content.AppendHtml(this.Header);
            tag.Content.AppendHtml(this.Body);
            foreach (TagHelperOutput item in this.Container.SkipLast(1).Reverse())
            {
                item.Content.SetHtmlContent(tag);
                if (item.Attributes.Any(o => o.Value.ToString() == "modal-content"))
                {
                    item.Content.AppendHtml(this.Footer);
                }

                tag = item;
            }

            tag.Content.AppendHtml(this.Script);
            return tag;
        }

        protected bool IsEdit => this.Model != null;

        protected TModel Model { get; }

        protected override IList<IHtmlContent> Container => new List<IHtmlContent>
        {
            AspNetCore.Extensions.TagHelper.Create(Tag.div, this.OutsideAttributes),
            AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-dialog modal-signup")),
            AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-content")),
            AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "card card-signup card-plain")),
        };

        protected override IHtmlContent Footer
        {
            get
            {
                var buttons = new List<ButtonColumn<TModel, TPostModel>>();
                this.CreateButtons(buttons);
                IList<IHtmlContent> list = new List<IHtmlContent>();
                foreach (var item in buttons)
                {
                    list.Add(item.Render(this.Model));
                }

                var footer = AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-footer"), list);
                return footer;
            }
        }

        protected override IHtmlContent Body
        {
            get
            {
                var columns = new List<LargeColumn<TModel, TPostModel>>();
                this.CreateColumns(columns);
                IList<IHtmlContent> list = new List<IHtmlContent>();
                foreach (var item in columns)
                {
                    list.Add(item.Render(this.Model));
                }

                var attributes1 = new TagAttributeList()
                {
                    { Attr.Type, "button" },
                    { Attr.Class, "form" },
                    { Attr.Method, "post" },
                    { Attr.Action, "/Schedule/Save" },
                };
                var form = AspNetCore.Extensions.TagHelper.Create(Tag.form, attributes1, list);
                var body = AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-body"), form);

                return body;
            }
        }

        protected override IHtmlContent Header
        {
            get
            {
                TagAttributeList attributes = new TagAttributeList()
                {
                    new TagAttribute(Attr.Type, "button"),
                    new TagAttribute(Attr.Class, "close"),
                    new TagAttribute(Attr.DataDismiss, "modal"),
                    new TagAttribute(Attr.AriaHidden, "true"),
                };
                var button = AspNetCore.Extensions.TagHelper.Create(Tag.button, attributes, new MaterialIcon("clear").Html);
                var h3 = AspNetCore.Extensions.TagHelper.Create(Tag.h3, new TagAttribute(Attr.Class, "modal-title card-title text-center"), this.Title);
                var header = AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "modal-header"), button, h3);
                return header;
            }
        }

        protected abstract void CreateColumns(IList<LargeColumn<TModel, TPostModel>> columns);

        protected abstract void CreateButtons(IList<ButtonColumn<TModel, TPostModel>> columns);
    }
}