namespace MaterialKit.Html.Buttons
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using MaterialKit.Html.Tags;
    using MaterialKit.Javascript;
    using Microsoft.AspNetCore.Html;

    public class SimpleButton
    {
        public SimpleButton(string labelText, string url = null)
        {
            this.Text = labelText;
            this.Url = url;
            this.Identifier = new Identifier();
            this.Script = new List<IHtmlContent>();
        }

        protected string Text { get; set; }

        protected string Func { get; set; }

        protected virtual string ButtonClass => "btn btn-simple";

        protected string Url { get; }

        protected virtual JavaScriptEvent Event => new JavaScriptEvent(func: this.Func, id: this.Identifier);

        protected virtual TagAttributeList Attributes
        {
            get
            {
                return new TagAttributeList()
                {
                    new TagAttribute(Attr.Type, "button"),
                    new TagAttribute(Attr.Class, this.ButtonClass),
                    new TagAttribute(Attr.Id, this.Identifier.Value),
                    new TagAttribute(Attr.Action, this.Url),
                };
            }
        }

        public IList<IHtmlContent> Script { get; set; }

        protected virtual Identifier Identifier { get; set; }

        public IHtmlContent Render()
        {
            var output = TagHelper.Create(Tag.button, this.Attributes, this.Text);
            if (this.Func != default)
            {
                this.Script.Add(this.Event.Script);
            }

            return output;
        }
    }
}
