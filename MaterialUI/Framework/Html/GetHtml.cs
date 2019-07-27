namespace Quartz.Html
{
    using System.IO;
    using Microsoft.AspNetCore.Hosting;

    public class GetHtml : IGetHtml
    {
        private readonly IHostingEnvironment env;

        public GetHtml(IHostingEnvironment env)
        {
            this.env = env;
        }

        public string GetContent(string fileName)
        {
            string file = Path.Combine((this.env.WebRootPath + fileName).Split('/', '\\'));
            var html = File.ReadAllText(file);
            return html;
        }
    }
}
