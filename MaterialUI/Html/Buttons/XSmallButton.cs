namespace MaterialKit.Html.Buttons
{
    public class XSmallButton : Button
    {
        public XSmallButton(string text)
            : base("btn btn-primary btn-xs", text)
        {
        }

        public static string Generate()
        {
            var button1 = new XSmallButton("x-Small");
            var button2 = new SmallButton("Small");
            var button3 = new PrimaryButton("Regular");
            var button4 = new LargeButton("Large");

            return button1.ToHtml() + "  " + button2.ToHtml() + button3.ToHtml() + button4.ToHtml();
        }
    }
}
