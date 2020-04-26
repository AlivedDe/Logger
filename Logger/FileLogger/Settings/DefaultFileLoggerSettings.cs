using System.Configuration;

namespace Logger.Settings
{
    public class DefaultFileLoggerSettings : LoggerSettings, IFileLoggerSettings
    {
        public DefaultFileLoggerSettings(LogLevel? logLevel = null) : base(logLevel)
        {
        }

        public string Directory { get; set; } = ConfigurationManager.AppSettings["LogFileDirectory"];

        public string FileName { get; set; } = "LogFile{0}.txt";
    }
}
