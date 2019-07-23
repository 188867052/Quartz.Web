namespace AppSettingManager
{
    public partial class LogLevel
    {
        public string Default;
    }

    public partial class Logging
    {
        public LogLevel LogLevel;
    }

    public partial class ConnectionStrings
    {
        public string DefaultConnection;
    }

    public partial class AppSettings
    {
        public Logging Logging;
        public ConnectionStrings ConnectionStrings;
        public string AllowedHosts;
        public string BaseAddress;
    }
}
