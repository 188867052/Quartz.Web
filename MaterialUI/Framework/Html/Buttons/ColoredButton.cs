namespace Quartz.Html.Buttons
{
    public class ColoredButton : Button
    {
        public ColoredButton(string text, string @class)
            : base(@class, text)
        {
        }

        public static string Generate()
        {
            var button1 = new ColoredButton("Default", IconClass.Default);
            var button2 = new ColoredButton("Primary", IconClass.Primary);
            var button3 = new ColoredButton("Info", IconClass.Info);
            var button4 = new ColoredButton("Success", IconClass.Success);
            var button5 = new ColoredButton("Warning", IconClass.Warning);
            var button6 = new ColoredButton("Danger", IconClass.Danger);
            var button7 = new ColoredButton("Rose", IconClass.Rose);

            return button1.ToHtml() + "  " + button2.ToHtml() + button3.ToHtml() + button4.ToHtml()
                 + button5.ToHtml() + button6.ToHtml() + button7.ToHtml();
        }
    }
}
