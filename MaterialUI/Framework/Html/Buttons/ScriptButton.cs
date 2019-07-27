namespace Quartz.Html.Buttons
{
    using Quartz.Shared;

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
