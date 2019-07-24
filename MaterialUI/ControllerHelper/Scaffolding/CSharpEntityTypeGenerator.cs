namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using JetBrains.Annotations;
    using MaterialUI.Shared;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Design.Internal;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class CSharpEntityTypeGenerator : ICSharpEntityTypeGenerator
    {
        private readonly ICSharpHelper _code;
        private bool _useDataAnnotations;
        protected IndentedStringBuilder _sb;

        public CSharpEntityTypeGenerator(
            [NotNull] ICSharpHelper cSharpHelper)
        {
            Check.NotNull(cSharpHelper, nameof(cSharpHelper));

            this._code = cSharpHelper;
        }

        protected virtual void GenerateNameSpace(IEntityType entityType)
        {
        }

        public virtual string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(@namespace, nameof(@namespace));

            this._sb = new IndentedStringBuilder();
            this._useDataAnnotations = useDataAnnotations;

            this._sb.AppendLine("using System;");
            this._sb.AppendLine("using System.Collections.Generic;");
            this.GenerateNameSpace(entityType);

            if (this._useDataAnnotations)
            {
                this._sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                this._sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            }

            foreach (var ns in entityType.GetProperties()
                .SelectMany(p => p.ClrType.GetNamespaces())
                .Where(ns => ns != "System" && ns != "System.Collections.Generic")
                .Distinct()
                .OrderBy(x => x, new NamespaceComparer()))
            {
                this._sb.AppendLine($"using {ns};");
            }

            this._sb.AppendLine();
            this._sb.AppendLine($"namespace {@namespace}");
            this._sb.AppendLine("{");

            using (this._sb.Indent())
            {
                this.GenerateClass(entityType);
            }

            this._sb.AppendLine("}");

            return this._sb.ToString();
        }

        protected virtual void GenerateClass(
            [NotNull] IEntityType entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            if (this._useDataAnnotations)
            {
                this.GenerateEntityTypeDataAnnotations(entityType);
            }

            this._sb.AppendLine($"public partial class {entityType.Name}");

            this._sb.AppendLine("{");

            using (this._sb.Indent())
            {
                this.GenerateConstructor(entityType);
                this.GenerateProperties(entityType);
                this.GenerateNavigationProperties(entityType);
            }

            this._sb.AppendLine("}");
        }

        protected virtual void GenerateEntityTypeDataAnnotations(
            [NotNull] IEntityType entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            this.GenerateTableAttribute(entityType);
        }

        private void GenerateTableAttribute(IEntityType entityType)
        {
            var tableName = entityType.Relational().TableName;
            var schema = entityType.Relational().Schema;
            var defaultSchema = entityType.Model.Relational().DefaultSchema;

            var schemaParameterNeeded = schema != null && schema != defaultSchema;
            var tableAttributeNeeded = schemaParameterNeeded || tableName != null && tableName != entityType.Scaffolding().DbSetName;

            if (tableAttributeNeeded)
            {
                var tableAttribute = new AttributeWriter(nameof(TableAttribute));

                tableAttribute.AddParameter(this._code.Literal(tableName));

                if (schemaParameterNeeded)
                {
                    tableAttribute.AddParameter($"{nameof(TableAttribute.Schema)} = {this._code.Literal(schema)}");
                }

                this._sb.AppendLine(tableAttribute.ToString());
            }
        }

        protected virtual void GenerateConstructor(
            [NotNull] IEntityType entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            var collectionNavigations = entityType.GetNavigations().Where(n => n.IsCollection()).ToList();

            if (collectionNavigations.Count > 0)
            {
                this._sb.AppendLine($"public {entityType.Name}()");
                this._sb.AppendLine("{");

                using (this._sb.Indent())
                {
                    foreach (var navigation in collectionNavigations)
                    {
                        this._sb.AppendLine($"{navigation.Name} = new HashSet<{navigation.GetTargetType().Name}>();");
                    }
                }

                this._sb.AppendLine("}");
                this._sb.AppendLine();
            }
        }

        protected virtual void GenerateProperties(
            [NotNull] IEntityType entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            foreach (var property in entityType.GetProperties().OrderBy(p => p.Scaffolding().ColumnOrdinal))
            {
                if (this._useDataAnnotations)
                {
                    this.GeneratePropertyDataAnnotations(property);
                }

                this._sb.AppendLine($"public {this.GetPropertyType(property)} {property.Name} {{ get; set; }}");
            }
        }

        protected virtual string GetPropertyType(IProperty property)
        {
            return this._code.Reference(property.ClrType);
        }

        protected virtual void GeneratePropertyDataAnnotations(
            [NotNull] IProperty property)
        {
            Check.NotNull(property, nameof(property));

            this.GenerateKeyAttribute(property);
            this.GenerateRequiredAttribute(property);
            this.GenerateColumnAttribute(property);
            this.GenerateMaxLengthAttribute(property);
        }

        private void GenerateKeyAttribute(IProperty property)
        {
            var key = property.AsProperty().PrimaryKey;

            if (key?.Properties.Count == 1)
            {
                if (key is Key concreteKey
                    && key.Properties.SequenceEqual(new KeyDiscoveryConvention(null).DiscoverKeyProperties(concreteKey.DeclaringEntityType, concreteKey.DeclaringEntityType.GetProperties().ToList())))
                {
                    return;
                }

                if (key.Relational().Name != ConstraintNamer.GetDefaultName(key))
                {
                    return;
                }

                this._sb.AppendLine(new AttributeWriter(nameof(KeyAttribute)));
            }
        }

        private void GenerateColumnAttribute(IProperty property)
        {
            var columnName = property.Relational().ColumnName;
            var columnType = property.GetConfiguredColumnType();

            var delimitedColumnName = columnName != null && columnName != property.Name ? this._code.Literal(columnName) : null;
            var delimitedColumnType = columnType != null ? this._code.Literal(columnType) : null;

            if ((delimitedColumnName ?? delimitedColumnType) != null)
            {
                var columnAttribute = new AttributeWriter(nameof(ColumnAttribute));

                if (delimitedColumnName != null)
                {
                    columnAttribute.AddParameter(delimitedColumnName);
                }

                if (delimitedColumnType != null)
                {
                    columnAttribute.AddParameter($"{nameof(ColumnAttribute.TypeName)} = {delimitedColumnType}");
                }

                this._sb.AppendLine(columnAttribute);
            }
        }

        private void GenerateMaxLengthAttribute(IProperty property)
        {
            var maxLength = property.GetMaxLength();

            if (maxLength.HasValue)
            {
                var lengthAttribute = new AttributeWriter(
                    property.ClrType == typeof(string)
                        ? nameof(StringLengthAttribute)
                        : nameof(MaxLengthAttribute));

                lengthAttribute.AddParameter(this._code.Literal(maxLength.Value));

                this._sb.AppendLine(lengthAttribute.ToString());
            }
        }

        private void GenerateRequiredAttribute(IProperty property)
        {
            if (!property.IsNullable
                && property.ClrType.IsNullableType()
                && !property.IsPrimaryKey())
            {
                this._sb.AppendLine(new AttributeWriter(nameof(RequiredAttribute)).ToString());
            }
        }

        protected virtual void GenerateNavigationProperties(
            [NotNull] IEntityType entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            var sortedNavigations = entityType.GetNavigations()
                .OrderBy(n => n.IsDependentToPrincipal() ? 0 : 1)
                .ThenBy(n => n.IsCollection() ? 1 : 0);

            if (sortedNavigations.Any())
            {
                this._sb.AppendLine();

                foreach (var navigation in sortedNavigations)
                {
                    if (this._useDataAnnotations)
                    {
                        this.GenerateNavigationDataAnnotations(navigation);
                    }

                    var referencedTypeName = navigation.GetTargetType().Name;
                    var navigationType = navigation.IsCollection() ? $"ICollection<{referencedTypeName}>" : referencedTypeName;
                    this._sb.AppendLine($"public {navigationType} {navigation.Name} {{ get; set; }}");
                }
            }
        }

        private void GenerateNavigationDataAnnotations(INavigation navigation)
        {
            this.GenerateForeignKeyAttribute(navigation);
            this.GenerateInversePropertyAttribute(navigation);
        }

        private void GenerateForeignKeyAttribute(INavigation navigation)
        {
            if (navigation.IsDependentToPrincipal())
            {
                if (navigation.ForeignKey.PrincipalKey.IsPrimaryKey())
                {
                    var foreignKeyAttribute = new AttributeWriter(nameof(ForeignKeyAttribute));

                    foreignKeyAttribute.AddParameter(
                        this._code.Literal(
                            string.Join(",", navigation.ForeignKey.Properties.Select(p => p.Name))));

                    this._sb.AppendLine(foreignKeyAttribute.ToString());
                }
            }
        }

        private void GenerateInversePropertyAttribute(INavigation navigation)
        {
            if (navigation.ForeignKey.PrincipalKey.IsPrimaryKey())
            {
                var inverseNavigation = navigation.FindInverse();

                if (inverseNavigation != null)
                {
                    var inversePropertyAttribute = new AttributeWriter(nameof(InversePropertyAttribute));

                    inversePropertyAttribute.AddParameter(this._code.Literal(inverseNavigation.Name));

                    this._sb.AppendLine(inversePropertyAttribute.ToString());
                }
            }
        }

        private class AttributeWriter
        {
            private readonly string _attibuteName;
            private readonly List<string> _parameters = new List<string>();

            public AttributeWriter([NotNull] string attributeName)
            {
                Check.NotEmpty(attributeName, nameof(attributeName));

                this._attibuteName = attributeName;
            }

            public void AddParameter([NotNull] string parameter)
            {
                Check.NotEmpty(parameter, nameof(parameter));

                this._parameters.Add(parameter);
            }

            public override string ToString()
                => "[" + (this._parameters.Count == 0
                       ? StripAttribute(this._attibuteName)
                       : StripAttribute(this._attibuteName) + "(" + string.Join(", ", this._parameters) + ")") + "]";

            private static string StripAttribute([NotNull] string attributeName)
                => attributeName.EndsWith("Attribute", StringComparison.Ordinal)
                    ? attributeName.Substring(0, attributeName.Length - 9)
                    : attributeName;
        }
    }
}
