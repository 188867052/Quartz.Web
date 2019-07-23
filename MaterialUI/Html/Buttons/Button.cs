namespace MaterialKit.Html.Buttons
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class Button
    {
        private readonly string @class;
        private readonly string text;
        private readonly string iconClass;

        public Button(string @class, string text, string iconClass = default)
        {
            Check.NotEmpty(@class, nameof(@class));

            this.@class = @class;
            this.text = text;
            this.iconClass = iconClass;
        }

        public string ToHtml()
        {
            TagHelperOutput button;
            TagAttribute attribute = new TagAttribute(Attr.Class, this.@class);
            if (!string.IsNullOrEmpty(this.iconClass))
            {
                button = AspNetCore.Extensions.TagHelper.Create(Tag.button, attribute, new MaterialIcon(this.iconClass, this.text).Html);
            }
            else
            {
                button = AspNetCore.Extensions.TagHelper.Create(Tag.button, attribute, this.text);
            }

            return AspNetCore.Extensions.TagHelper.ToHtml(button);
        }

        public static string Generate()
        {
            var button1 = new PrimaryButton("Default");
            var button2 = new RoundButton("Round");
            var button3 = new IconRoundButton("favorite", " With Icon");
            var button4 = new IconMiniButton("favorite");
            var button5 = new SimpleButton("Simple");

            return button1.ToHtml() + "  " + button2.ToHtml() + button3.ToHtml() + button4.ToHtml() + button5.Render();
        }
    }
}
