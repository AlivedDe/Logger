namespace Logger
{
    public abstract class LoggerSettings : ILoggerSettings
    {
        public LoggerSettings(LogLevel? logLevel = null)
        {
            LogLevel = logLevel ?? LogLevel.Info;
        }

        public LoggerSettings(ILoggerSettings loggerSettings) : this(loggerSettings?.LogLevel)
        {
        }

        public LogLevel LogLevel { get; set; }

        public string MessageFormat { get; set; } = "{0}\t{1:yyyy-MM-dd_HH:mm:ss}:\t{2}";
    }
}
