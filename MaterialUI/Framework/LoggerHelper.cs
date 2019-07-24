namespace MaterialUI.Framework
{
    using System;
    using Serilog;
    using Serilog.Configuration;
    using Serilog.Events;

    public static class LoggerHelper
    {
        public static void LogConfig()
        {
            Log.Logger = new LoggerConfiguration()
                                 .Enrich.FromLogContext()
                                 .MinimumLevel.Debug()
                                 .MinimumLevel.Override(nameof(System), LogEventLevel.Information)
                                 .MinimumLevel.Override(nameof(Microsoft), LogEventLevel.Information)
                                 .LoggerConfigure(LogEventLevel.Debug)
                                 .LoggerConfigure(LogEventLevel.Information)
                                 .LoggerConfigure(LogEventLevel.Warning)
                                 .LoggerConfigure(LogEventLevel.Error)
                                 .LoggerConfigure(LogEventLevel.Fatal)
                                 .CreateLogger();
        }

        private static Action<LoggerSinkConfiguration> ConfigureDelegate(LogEventLevel level) => a => a.RollingFile($"File/logs/log-{{Date}}-{level.ToString()}.txt");

        private static Action<LoggerConfiguration> LoggerDelegate(LogEventLevel level) => lg => lg.Filter.ByIncludingOnly(p => p.Level == level).WriteTo.Async(a => ConfigureDelegate(level));

        private static LoggerConfiguration LoggerConfigure(this LoggerConfiguration logger, LogEventLevel level) => logger.WriteTo.Logger(LoggerDelegate(level));
    }
}