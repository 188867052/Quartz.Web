namespace MaterialUI.Html.Buttons
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;
    using MaterialUI.Javascript;

    public class ModalButton : SimpleButton
    {
        private readonly Identifier id;

        public ModalButton(string labelText, string url, Identifier id)
           : base(labelText, url)
        {
            this.id = id;
        }

        protected override TagAttributeList Attributes
        {
            get
            {
                return new TagAttributeList()
                {
                    new TagAttribute(Attr.Type, "button"),
                    new TagAttribute(Attr.Rel, "tooltip"),
                    new TagAttribute(Attr.Class, this.ButtonClass),
                    new TagAttribute(Attr.Action, this.Url),
                    new TagAttribute(Attr.DataToggle, "modal"),
                    new TagAttribute(Attr.DataTarget, $"#{this.id.Value}"),
                };
            }
        }

        protected override string ButtonClass => "btn btn-rose";
    }
}
