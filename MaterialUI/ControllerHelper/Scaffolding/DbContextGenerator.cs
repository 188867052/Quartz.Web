namespace MaterialUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using AppSettingManager;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
    using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
    using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Services
    {
        internal static T GetService<T>(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetRequiredService<T>();
        }
    }

    internal class DbContextGenerator
    {
        private readonly string directory;

        internal string DbContextCode { get; set; }

        internal IList<string> EntityCodeList { get; set; }

        internal DbContextGenerator(string folder = "")
        {
            this.EntityCodeList = new List<string>();
            this.directory = string.IsNullOrEmpty(folder) ? Environment.CurrentDirectory : Path.Combine(Environment.CurrentDirectory, folder);

            IServiceCollection services = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<ICSharpDbContextGenerator, MyDbContextGenerator>()
                .AddSingleton<ICSharpEntityTypeGenerator, MyEntityTypeGenerator>()
                .AddSingleton<IScaffoldingModelFactory, MyScaffoldingModelFactory>();

            new SqlServerDesignTimeServices().ConfigureDesignTimeServices(services);
            var logger = services.GetService<IDiagnosticsLogger<DbLoggerCategory.Scaffolding>>();
            var databaseModelFactory = new SqlServerDatabaseModelFactory(logger);
            MyDbContextGenerator dbContextGenerator = (MyDbContextGenerator)services.GetService<ICSharpDbContextGenerator>();
            MyEntityTypeGenerator entityTypeGenerator = (MyEntityTypeGenerator)services.GetService<ICSharpEntityTypeGenerator>();
            var scaffoldingModelFactory = (MyScaffoldingModelFactory)services.GetService<IScaffoldingModelFactory>();
            var databaseModel = databaseModelFactory.Create(AppSettings.Connection, new List<string>(), new List<string>());
            Model model = (Model)scaffoldingModelFactory.Create(databaseModel, false);
            this.DbContextCode = dbContextGenerator.WriteCode(model, $"{nameof(MaterialUI)}.{folder}", $"{nameof(MaterialUI)}Context", AppSettings.Connection, false, false);
            foreach (var entityType in model.GetEntityTypes())
            {
                var entityCode = entityTypeGenerator.WriteCode(entityType, $"{nameof(MaterialUI)}.{folder}", false);
                this.EntityCodeList.Add(entityCode);
            }

            this.WriteCode(scaffoldingModelFactory.Data, $"{nameof(MaterialUI)}.{folder}");
        }

        internal void WriteCode(Dictionary<string, string> dictionary, string @namespace)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"namespace {@namespace}");
            sb.AppendLine("{");
            sb.AppendLine("    using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine($"    public partial class MetaData");
            sb.AppendLine("    {");
            sb.AppendLine("        public static Dictionary<string, string> Mapping = new Dictionary<string, string>");
            sb.AppendLine("        {");
            foreach (var data in dictionary)
            {
                sb.AppendLine($"            {{ \"{data.Key}\", \"{data.Value}\" }},");
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
            return Path.Combine(this.directory, name + ".cs");
        }
    }
}
