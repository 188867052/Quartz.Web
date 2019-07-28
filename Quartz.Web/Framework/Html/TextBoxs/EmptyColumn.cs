namespace Quartz.Html.TextBoxs
{
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Javascript;

    public class EmptyColumn<TModel, TPostModel> : IColumn<TModel, TPostModel>
    {
        public ComulnWidth Width { get; set; } = ComulnWidth.Default;

        public IHtmlContent Render(TModel entity)
        {
            return TagHelper.Combine(string.Empty);
        }
    }
}