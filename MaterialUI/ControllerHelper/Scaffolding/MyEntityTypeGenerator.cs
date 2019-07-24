namespace MaterialUI.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using JetBrains.Annotations;
    using MaterialUI.Scaffolding;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

    internal class MyEntityTypeGenerator
        : CSharpEntityTypeGenerator
    {
        private readonly ScaffoldConfig scaffoldConfig;

        public MyEntityTypeGenerator([NotNull] ICSharpHelper cSharpHelper)
            : base(cSharpHelper)
        {
            this.scaffoldConfig = this.GetScaffoldConfig(Environment.CurrentDirectory);
        }

        protected override string GetPropertyType(IProperty property)
        {
            string typeName = base.GetPropertyType(property);
            var propertyImp = (Property)property;
            var fieldConfig = this.scaffoldConfig?.Tables?.FirstOrDefault(o => o.Name == propertyImp?.DeclaringType?.Name)?.Fields?.FirstOrDefault(o => o.Name == property.Name);
            if (fieldConfig != null)
            {
                return fieldConfig.Type;
            }

            return typeName;
        }

        protected override void GenerateNameSpace(IEntityType entityType)
        {
            var table = this.scaffoldConfig.Tables.FirstOrDefault(o => o.Name == entityType.Name);
            if (table != null)
            {
                foreach (var property in table.Fields.Where(property => !string.IsNullOrEmpty(property.HasConversion)).Select(property => property))
                {
                    Namespace @namespace = this.scaffoldConfig.Namespaces.FirstOrDefault(o => o.Name == property.Type);
                    if (@namespace != null)
                    {
                        this._sb.AppendLine($"using {@namespace.Value};");
                    }
                }
            }
        }

        private ScaffoldConfig GetScaffoldConfig(string webRootPath)
        {
            DirectoryInfo di = new DirectoryInfo(webRootPath);
            var file = di.GetFiles("Scaffolding.xml", SearchOption.AllDirectories).FirstOrDefault();
            var xml = File.ReadAllText(file.FullName);
            return this.Deserialize(xml);
        }

        private ScaffoldConfig Deserialize(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(ScaffoldConfig));
                return (ScaffoldConfig)xmldes.Deserialize(sr);
            }
        }
    }
}
