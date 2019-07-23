namespace MaterialUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using AppSettingManager;
    using MaterialUI.Scaffolding;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
    using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
    using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
    using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
    using Microsoft.Extensions.DependencyInjection;

    internal static class ScaffoldingHelper
    {
        internal static IList<string> Scaffolding(string contentRootPath, string folder)
        {
            DbContextGenerator generator = new DbContextGenerator(folder);
            var config = generator.GetScaffoldConfig(contentRootPath);
            foreach (var entity in generator.ModelBuilderEntities)
            {
                string newEntity = entity;
                string entityName = Regex.Match(entity, "<[^>]+>").Value.Trim('<', '>');

                var entityConfig = config.Tables.FirstOrDefault(o => o.Name.Equals(entityName, StringComparison.InvariantCultureIgnoreCase));
                if (entityConfig == null)
                {
                    continue;
                }

                var properties = Regex.Matches(entity, @"entity.Property\(e => e.[^.]+");
                foreach (Match property in properties)
                {
                    string propertyName = Regex.Match(property.Value, @"[.][^.]+\)").Value.Trim('.', ')');
                    var fieldConfig = entityConfig.Fields.FirstOrDefault(o => o.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

                    var value = property.Value.Replace(Environment.NewLine, "").TrimEnd();
                    string newProperty;
                    switch (fieldConfig?.HasConversion)
                    {
                        case "int2enum":
                            newProperty = $@"{value}
                    .HasConversion(v => (int)v, v => ({fieldConfig.Type})v)";
                            break;
                        case "string2enum":
                            newProperty = $@"{value}
                    .HasConversion(v => v.ToString(), v => ({fieldConfig.Type})Enum.Parse(typeof({fieldConfig.Type}), v))";
                            break;
                        default:
                            continue;
                    }

                    newEntity = newEntity.Replace(property.Value.Trim(), newProperty);
                }

                generator.DbContextCode = generator.DbContextCode.Replace(entity, newEntity);
            }

            generator.DbContextCode = InsertBlankLine(generator.DbContextCode);
            foreach (var table in config.Tables)
            {
                foreach (var property in table.Fields)
                {
                    if (!string.IsNullOrEmpty(property.HasConversion))
                    {
                        string perperty = generator.GetPerpertyCodeByName(table.Name, property.Name);
                        var list = perperty.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        string oldType = list[1];
                        string newField = perperty.Replace(oldType, property.Type);
                        var oldCode = generator.EntityCodeList.FirstOrDefault(o => generator.Predicate(o, table.Name));
                        var newCode = oldCode.Replace(perperty, newField);
                        generator.EntityCodeList.Remove(oldCode);
                        generator.EntityCodeList.Add(newCode);

                        Namespace @namespace = config.Namespaces.FirstOrDefault(o => o.Name == property.Type);
                        if (@namespace != null)
                        {
                            generator.AddNamespaces(table.Name, @namespace.Value);
                        }
                    }
                }
            }

            IList<string> listCode = new List<string>();
            foreach (var item in generator.EntityCodeList)
            {
                var newCode = InsertBlankLine(item);
                listCode.Add(newCode);
            }

            generator.EntityCodeList = listCode;
            generator.WriteTo();
            IList<string> changedFiles = generator.EntityCodeList;
            changedFiles.Add(generator.DbContextCode);

            return changedFiles;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1116:Split parameters should start on line after declaration", Justification = "<挂起>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "<挂起>")]
        internal static string InsertBlankLine(string code)
        {
            code = code.Replace("{ get; set; }", "{ get; set; }" + $"\r\n").Replace(@"{ get; set; }


", @"{ get; set; }

").Replace(@"
    }", "    }");
            return code;
        }
    }

    internal class DbContextGenerator
    {
        private readonly string directory;

        internal string DbContextCode { get; set; }

        internal IList<string> EntityCodeList { get; set; }

        internal IList<string> ModelBuilderEntities { get; set; }

        internal DbContextGenerator(string folder = "")
        {
            this.directory = Path.Combine(Environment.CurrentDirectory, folder);

            IServiceCollection services = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .AddEntityFrameworkDesignTimeServices();

            new SqlServerDesignTimeServices().ConfigureDesignTimeServices(services);
            var logger = services.GetService<IDiagnosticsLogger<DbLoggerCategory.Scaffolding>>();
            var databaseModelFactory = new SqlServerDatabaseModelFactory(logger);
            var dbContextGenerator = services.GetService<ICSharpDbContextGenerator>();
            var entityTypeGenerator = services.GetService<ICSharpEntityTypeGenerator>();
            var scaffoldingModelFactory = services.GetService<IScaffoldingModelFactory>();
            var databaseModel = databaseModelFactory.Create(AppSettings.Connection, new List<string>(), new List<string>());
            Model model = (Model)scaffoldingModelFactory.Create(databaseModel, false);

            this.DbContextCode = dbContextGenerator.WriteCode(model, $"MaterialKit.{folder}", "MaterialKitContext", AppSettings.Connection, false, false);
            this.EntityCodeList = new List<string>();
            this.ModelBuilderEntities = new List<string>();
            foreach (var entityType in model.GetEntityTypes())
            {
                var entityCode = entityTypeGenerator.WriteCode(entityType, $"MaterialKit.{folder}", false);
                this.EntityCodeList.Add(entityCode);
            }

            var match = Regex.Match(this.DbContextCode, "modelBuilder.Entity(.|\n)+");
            var value = match.Value;
            this.ModelBuilderEntities = value.Split($"modelBuilder.{folder}", StringSplitOptions.RemoveEmptyEntries);
            if (string.IsNullOrEmpty(folder))
            {
                this.directory = Environment.CurrentDirectory;
            }
            else
            {
                this.directory = Path.Combine(Environment.CurrentDirectory, folder);
            }

            this.WriteCode(databaseModel, $"MaterialKit.{folder}");
        }

        internal void WriteCode(DatabaseModel databaseModel, string @namespace)
        {
            IList<string> entityNames = new List<string>();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"namespace {@namespace}");
            sb.AppendLine("{");
            sb.AppendLine("    using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine($"    public partial class MetaData");
            sb.AppendLine("    {");
            var matches = Regex.Matches(this.DbContextCode, "modelBuilder.Entity<.+>");
            foreach (Match item in matches)
            {
                string value = item.Value.Split('<', '>')[1];
                entityNames.Add(value);
            }

            entityNames = entityNames.OrderBy(o => o).ToList();
            sb.AppendLine("        public static Dictionary<string, string> Mapping = new Dictionary<string, string>");
            sb.AppendLine("        {");
            var tables = databaseModel.Tables.OrderBy(o => o.Name).ToList();
            foreach (var item in tables)
            {
                string entityName = entityNames[tables.IndexOf(item)];
                sb.AppendLine($"            {{ \"{entityName}\", \"{item.Name}\" }},");
            }

            sb.AppendLine("        };");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            if (!Directory.Exists(this.directory))
            {
                Directory.CreateDirectory(this.directory);
            }

            string code = sb.ToString();
            File.WriteAllText(this.GetPath(code), code, Encoding.UTF8);
        }

        internal string GetEntityCodeByName(string entityName)
        {
            return this.EntityCodeList.FirstOrDefault(o => o.Contains($"public partial class {entityName}"));
        }

        internal bool Predicate(string o, string e) => o.Contains($"public partial class {e}");

        internal string GetPerpertyCodeByName(string entityName, string perpertyName)
        {
            var entityCode = this.EntityCodeList.FirstOrDefault(o => this.Predicate(o, entityName));
            string perperty = Regex.Match(entityCode, $"public[ ][^ ]+[ ]{perpertyName} {{ get; set; }}").Value;
            return perperty;
        }

        internal void AddNamespaces(string entityName, string @namespace)
        {
            bool isDbContext = string.IsNullOrEmpty(this.GetEntityCodeByName(entityName));
            var oldEntityCode = isDbContext
                ? this.DbContextCode
                : this.EntityCodeList.FirstOrDefault(o => this.Predicate(o, entityName));

            var matches = Regex.Matches(oldEntityCode, $"using [^  ()]+;");

            var oldList = matches.Select(o => o.Value).ToList();
            var newList = new List<string>().Union(oldList).ToList();

            // TODO:add or update.
            newList.Add("using " + @namespace + ";");
            var newEntityCode = oldEntityCode.Replace(string.Join(Environment.NewLine, oldList), string.Join(Environment.NewLine, newList));

            if (isDbContext)
            {
                this.DbContextCode = newEntityCode;
            }
            else
            {
                this.AddNamespaces("MaterialKitContext", @namespace);
                this.EntityCodeList.Remove(oldEntityCode);
                this.EntityCodeList.Add(newEntityCode);
            }
        }

        internal void WriteTo()
        {
            if (!Directory.Exists(this.directory))
            {
                Directory.CreateDirectory(this.directory);
            }

            File.WriteAllText(this.GetPath(this.DbContextCode), this.DbContextCode, Encoding.UTF8);
            foreach (var entity in this.EntityCodeList)
            {
                File.WriteAllText(this.GetPath(entity), entity, Encoding.UTF8);
            }
        }

        private string GetPath(string @class)
        {
            string name = Regex.Match(@class, $"public partial class [^ ]+").Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Last().Trim();
            string filePath = Path.Combine(this.directory, name + ".cs");
            return filePath;
        }

        internal ScaffoldConfig GetScaffoldConfig(string webRootPath)
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

        private string Serialize<T>(T config)
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

    internal static class Services
    {
        internal static T GetService<T>(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetRequiredService<T>();
        }
    }
}
