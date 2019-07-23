namespace MaterialUI.Html.TextBoxs
{
    using AspNetCore.Extensions;
    using MaterialUI.Javascript;
    using Microsoft.AspNetCore.Html;

    public class EmptyColumn<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        public ComulnWidth Width { get; set; } = ComulnWidth.Default;

        public IHtmlContent Render(TModel entity)
        {
            return TagHelper.Combine(string.Empty);
        }
    }
}