namespace MaterialUI.Javascript
{
    using Microsoft.AspNetCore.Html;

    public interface IColumn<TModel, TPostModel>
    {
        IHtmlContent Render(TModel tModel);

        ComulnWidth Width { get; set; }
    }

    public enum ComulnWidth
    {
        Default = 5,
        Four = 4,
        Three = 3,
        Two = 2,
        One = 1,
    }
}
