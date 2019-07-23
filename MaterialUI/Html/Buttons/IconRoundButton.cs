namespace MaterialKit.Html.Buttons
{
    public class IconRoundButton : Button
    {
        public IconRoundButton(string iconClass, string text = default)
            : base("btn btn-primary btn-round", text, iconClass)
        {
        }
    }
}
