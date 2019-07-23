namespace AspNetCore.Extensions.JsonClassGenerate
{
    using System;
    using System.IO;
    using System.Linq;

    public static class GenerateAppSetting
    {
        public static string Generate()
        {
            string _namespace = "AppSetting";
            string _mainClassName = "AppSettings";
            string _targetFolder = Environment.CurrentDirectory;
            bool _pascalCase = false;
            bool _singleFile = true;
            DirectoryInfo di = new DirectoryInfo(_targetFolder);
            var file = di.GetFiles("appsettings.json", SearchOption.AllDirectories).FirstOrDefault();

            var jsonFile = File.ReadAllText(file.FullName);
            var gen = new JsonClassGenerator
            {
                Namespace = _namespace,
                TargetFolder = _targetFolder,
                MainClass = _mainClassName,
                UsePascalCase = _pascalCase,
                SingleFile = _singleFile,
                Example = jsonFile,
            };

            using (var sw = new StringWriter())
            {
                gen.OutputStream = sw;
                var generatedCode = gen.GenerateClasses();
                sw.Flush();

                var appSettingsFiles = di.GetFiles("AppSettings.cs", SearchOption.AllDirectories).FirstOrDefault();
                File.WriteAllText(appSettingsFiles.FullName, generatedCode);
                return generatedCode;
            }
        }
    }
}
