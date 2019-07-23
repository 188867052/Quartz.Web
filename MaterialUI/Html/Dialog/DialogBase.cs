﻿namespace MaterialKit.Html.Dialog
{
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using Javascript;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public abstract class DialogBase
    {
        protected TagAttributeList OutsideAttributes => new TagAttributeList()
        {
            new TagAttribute(Attr.Class, "modal fade"),
            new TagAttribute(Attr.Id, $"{this.Identifier.Value}"),
            new TagAttribute(Attr.TabIndex, "-1"),
            new TagAttribute(Attr.Role, "dialog"),
            new TagAttribute(Attr.AriaLabelledBy, "myModalLabel"),
            new TagAttribute(Attr.AriaHidden, "true"),
        };

        public abstract string Title { get; }

        public abstract Identifier Identifier { get; }

        public IHtmlContent Script
        {
            get
            {
                var js = AspNetCore.Extensions.TagHelper.Create(Tag.script, "materialKit.initFormExtendedDatetimepickers();");
                js.Content.AppendHtml("$(\"[type = 'submit']\").on(\"click\", function (e) { index.submit(e) })");
                return js;
            }
        }

        public virtual IHtmlContent Header
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
                var h4 = AspNetCore.Extensions.TagHelper.Create(Tag.h4, new TagAttribute(Attr.Class, "modal-title title"), this.Title);
                var header = AspNetCore.Extensions.TagHelper.Div("modal-header", button, h4);
                return header;
            }
        }

        public virtual IHtmlContent Body
        {
            get
            {
                var p = AspNetCore.Extensions.TagHelper.Create(Tag.p, "Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth. Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic life One day however a small line of blind text by the name of Lorem Ipsum decided to leave for the far World of Grammar.");
                var body = AspNetCore.Extensions.TagHelper.Div("modal-body", p);
                return body;
            }
        }

        public abstract IHtmlContent Footer { get; }

        public virtual int Width { get; } = 50;

        public virtual IList<IHtmlContent> Container => new List<IHtmlContent>
        {
            AspNetCore.Extensions.TagHelper.Create(Tag.div, this.OutsideAttributes),
            AspNetCore.Extensions.TagHelper.Create(Tag.div, new TagAttributeList { { Attr.Class, "modal-dialog" }, { Attr.Style, $"width: {this.Width}%" } }),
            AspNetCore.Extensions.TagHelper.Div("modal-content"),
        };

        public virtual IHtmlContent Render()
        {
            Check.NotEmpty(this.Container.ToList(), nameof(this.Container));

            var tag = (TagHelperOutput)this.Container.Last();
            tag.Content.AppendHtml(this.Header);
            tag.Content.AppendHtml(this.Body);
            tag.Content.AppendHtml(this.Footer);

            foreach (TagHelperOutput item in this.Container.SkipLast(1).Reverse())
            {
                item.Content.SetHtmlContent(tag);
                tag = item;
            }

            tag.Content.AppendHtml(this.Script);
            return tag;
        }
    }
}