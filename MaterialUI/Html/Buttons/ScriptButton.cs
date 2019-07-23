namespace MaterialUI.Html.Buttons
{
    using AspNetCore.Extensions;

    public class ScriptButton : SimpleButton
    {
        public ScriptButton(string labelText, string func, string url)
           : base(labelText, url)
        {
            Check.NotEmpty(func, nameof(func));

            this.Func = func;
        }

        protected override string ButtonClass => "btn btn-script btn-rose";
    }
}
