namespace MaterialUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MaterialUI.Scaffolding;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.EntityFrameworkCore.Scaffolding;
    using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

    internal class MyDbContextGenerator : CSharpDbContextGeneratorBase
    {
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
        }

        protected override void GenerateProperty(IProperty property, bool useDataAnnotations)
        {
            base.GenerateProperty(property, useDataAnnotations);
        }

        protected override void GenerateNameSpace()
        {
            foreach (var property in Helper.ScaffoldConfig.Entities.SelectMany(table => table.Properties.Where(property => property.HasConversion).Select(property => property).Select(property => property)))
            {
                Namespace ns = Helper.ScaffoldConfig.Namespaces.FirstOrDefault(o => o.Name == property.Type);
                if (ns != null)
                {
                    string us = $"using {ns.Value};";
                    if (!this.sb.ToString().Contains(us))
                    {
                        this.sb.AppendLine(us);
                    }
                }
            }
        }

        protected override List<string> Lines(IProperty property)
        {
            var line = base.Lines(property);
            var propertyImp = (Microsoft.EntityFrameworkCore.Metadata.Internal.Property)property;
            var fieldConfig = Helper.ScaffoldConfig?.Entities?.FirstOrDefault(o => o.Name == propertyImp?.DeclaringType?.Name)?.Properties?.FirstOrDefault(o => o.Name == property.Name);
            switch (fieldConfig?.DbType)
            {
                case "int":
                    switch (fieldConfig?.CSharpType)
                    {
                        default:
                            line.Add($@".HasConversion(v => (int)v, v => ({fieldConfig.Type})v)");
                            break;
                    }

                    break;
                case "string":
                    switch (fieldConfig?.CSharpType)
                    {
                        default:
                            line.Add($@".HasConversion(v => v.ToString(), v => ({fieldConfig.Type})Enum.Parse(typeof({fieldConfig.Type}), v))");
                            break;
                    }

                    break;

                case "long":
                case "long?":
                    switch (fieldConfig?.CSharpType)
                    {
                        case "DateTime":
                            line.Add($@".HasConversion(v => v.Ticks, v => new DateTime(v))");
                            break;
                        case "DateTime?":
                            line.Add($@".HasConversion(v => v.Value.Ticks, v => new DateTime(v))");
                            break;
                    }

                    break;
            }

            return line;
        }
    }
}
