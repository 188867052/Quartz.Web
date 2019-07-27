namespace Quartz.Controllers
{
    using System.Linq;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
    using Quartz.Scaffolding;

    internal class MyEntityTypeGenerator
        : CSharpEntityTypeGeneratorBase
    {
        public MyEntityTypeGenerator([NotNull] ICSharpHelper cSharpHelper)
            : base(cSharpHelper)
        {
        }

        protected override string GetPropertyType(IProperty property)
        {
            string typeName = base.GetPropertyType(property);
            var propertyImp = (Microsoft.EntityFrameworkCore.Metadata.Internal.Property)property;
            var fieldConfig = Helper.ScaffoldConfig?.Entities?.FirstOrDefault(o => o.Name == propertyImp?.DeclaringType?.Name)?.Properties?.FirstOrDefault(o => o.Name == property.Name);
            if (fieldConfig != null)
            {
                return fieldConfig.Type;
            }

            return typeName;
        }

        protected override void GenerateNameSpace(IEntityType entityType)
        {
            var table = Helper.ScaffoldConfig.Entities.FirstOrDefault(o => o.Name == entityType.Name);
            if (table != null)
            {
                foreach (var property in table.Properties.Where(property => property.HasConversion).Select(property => property))
                {
                    Namespace ns = Helper.ScaffoldConfig.Namespaces.FirstOrDefault(o => o.Name == property.Type);
                    if (ns != default)
                    {
                        string us = $"using {ns.Value};";
                        if (!this.IndentedStringBuilder.ToString().Contains(us))
                        {
                            this.IndentedStringBuilder.AppendLine(us);
                        }
                    }
                }
            }
        }
    }
}
