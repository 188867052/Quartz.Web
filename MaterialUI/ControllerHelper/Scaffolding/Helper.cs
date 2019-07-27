namespace Quartz.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz.Scaffolding;

    internal static class Helper
    {
        static Helper()
        {
            ScaffoldConfig = GetScaffoldConfig(Environment.CurrentDirectory);
        }

        internal static ScaffoldConfig ScaffoldConfig { get; }

        internal static T GetService<T>(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetRequiredService<T>();
        }

        private static ScaffoldConfig GetScaffoldConfig(string webRootPath)
        {
            DirectoryInfo di = new DirectoryInfo(webRootPath);
            var file = di.GetFiles("Scaffolding.xml", SearchOption.AllDirectories).FirstOrDefault();
            var xml = File.ReadAllText(file.FullName);
            return Deserialize(xml);
        }

        private static ScaffoldConfig Deserialize(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(ScaffoldConfig));
                return (ScaffoldConfig)xmldes.Deserialize(sr);
            }
        }
    }
}
