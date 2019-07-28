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
            var scaffoldConfig = Deserialize(xml);

            // Formatting the xml.
            var newConfig = scaffoldConfig;
            newConfig.Entities = scaffoldConfig.Entities.OrderBy(o => o.Name).ToArray();
            newConfig.Namespaces = scaffoldConfig.Namespaces.OrderBy(o => o.Value).ToArray();
            string xmlSerialized = Serialize(newConfig);
            File.WriteAllText(file.FullName, xmlSerialized);

            return scaffoldConfig;
        }

        private static ScaffoldConfig Deserialize(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(ScaffoldConfig));
                return (ScaffoldConfig)xmldes.Deserialize(sr);
            }
        }

        private static string Serialize<T>(T config)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(writer, config, namespaces);
                return writer.ToString();
            }
        }
    }
}
