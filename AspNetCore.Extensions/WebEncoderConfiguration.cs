namespace Core.Extension.DependencyInjection
{
    using System.Text.Encodings.Web;
    using System.Text.Unicode;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.WebEncoders;

    public static class WebEncoderConfiguration
    {
        public static void AddService(IServiceCollection services)
        {
            services.Configure<WebEncoderOptions>(o => o.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs));
        }
    }
}