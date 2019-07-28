namespace AppSettingManager
{
    using System;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public partial class AppSettings
    {
        static AppSettings()
        {
            var file = Directory.GetFiles(Environment.CurrentDirectory, "appsettings.json", SearchOption.AllDirectories).FirstOrDefault();
            Instance = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(file));
        }

        public static AppSettings Instance { get; }

        public static string Connection => Instance.ConnectionStrings.DefaultConnection;
    }
}