using System;

namespace Logger.Settings
{
    public class DefaultConsoleLoggerSettings : LoggerSettings, IConsoleLoggerSettings
    {
        public DefaultConsoleLoggerSettings(LogLevel? logLevel = null) : base(logLevel)
        {
        }

        public ConsoleColor ErrorForeColor { get; set; } = ConsoleColor.Red;

        public ConsoleColor WarningForeColor { get; set; } = ConsoleColor.Yellow;

        public ConsoleColor InfoForeColor { get; set; } = ConsoleColor.White;
    }
}
