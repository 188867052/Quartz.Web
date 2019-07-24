namespace MaterialUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using MaterialUI.Scaffolding;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.EntityFrameworkCore.Scaffolding;
    using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

    internal class MyDbContextGenerator : CSharpDbContextGenerator
    {
        private readonly ScaffoldConfig scaffoldConfig;

        [Obsolete]
        public MyDbContextGenerator(
            IEnumerable<IScaffoldingProviderCodeGenerator> legacyProviderCodeGenerators,
            IEnumerable<IProviderConfigurationCodeGenerator> providerCodeGenerators,
            IAnnotationCodeGenerator annotationCodeGenerator,
            ICSharpHelper cSharpHelper)
            : base(
                legacyProviderCodeGenerators,
                providerCodeGenerators,
                annotationCodeGenerator,
                cSharpHelper)
        {
            this.scaffoldConfig = this.GetScaffoldConfig(Environment.CurrentDirectory);
        }

        protected override void GenerateProperty(IProperty property, bool useDataAnnotations)
        {
            base.GenerateProperty(property, useDataAnnotations);
        }

        protected override void GenerateNameSpace()
        {
            foreach (var property in this.scaffoldConfig.Tables.SelectMany(table => table.Fields.Where(property => !string.IsNullOrEmpty(property.HasConversion)).Select(property => property).Select(property => property)))
            {
                Namespace @namespace = this.scaffoldConfig.Namespaces.FirstOrDefault(o => o.Name == property.Type);
                if (@namespace != null)
                {
                    this._sb.AppendLine($"using {@namespace.Value};");
                }
            }
        }

        protected override List<string> lines(IProperty property)
        {
            var line = base.lines(property);
            var propertyImp = (Property)property;
            var fieldConfig = this.scaffoldConfig?.Tables?.FirstOrDefault(o => o.Name == propertyImp?.DeclaringType?.Name)?.Fields?.FirstOrDefault(o => o.Name == property.Name);
            switch (fieldConfig?.HasConversion)
            {
                case "int2enum":
                    line.Add($@".HasConversion(v => (int)v, v => ({fieldConfig.Type})v)");
                    break;
                case "string2enum":
                    line.Add($@".HasConversion(v => v.ToString(), v => ({fieldConfig.Type})Enum.Parse(typeof({fieldConfig.Type}), v))");
                    break;
            }

            return line;
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
