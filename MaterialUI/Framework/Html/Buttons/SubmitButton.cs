namespace Quartz.Html.Buttons
{
    using AspNetCore.Extensions;
    using Newtonsoft.Json;
    using Quartz.Html.Tags;

    public class SubmitButton : SimpleButton
    {
        private readonly object data;

        public SubmitButton(string labelText, string func, string action, object data)
            : base(labelText, action)
        {
            this.data = data;
            this.Func = func;
        }

        protected override TagAttributeList Attributes
        {
            get
            {
                return new TagAttributeList()
                {
                    new TagAttribute(Attr.Type, "button"),
                    new TagAttribute(Attr.Class, this.ButtonClass),
                    new TagAttribute(Attr.Id, this.Identifier.Value),
                    new TagAttribute(Attr.Action, this.Url),
                    new TagAttribute(Attr.Data, JsonConvert.SerializeObject(this.data)),
                };
            }
        }

        protected override string ButtonClass => "btn btn-success btn-simple";
    }
}
