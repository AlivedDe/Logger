using System.Configuration;

namespace Logger.Settings
{
    public class DefaultSqlLoggerSettings : LoggerSettings, ISqlLoggerSettings
    {
        public DefaultSqlLoggerSettings(LogLevel? logLevel = null) : base(logLevel)
        {
        }

        public string ConnectionString { get; set; } = ConfigurationManager.AppSettings["ConnectionString"];
    }
}
