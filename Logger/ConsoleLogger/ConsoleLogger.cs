using System;
using System.Threading.Tasks;

namespace Logger
{
    /// <summary>
    /// Logger for dispaying messages into console
    /// </summary>
    public class ConsoleLogger : AbstractLogger, ILogger
    {
        private readonly IConsoleLoggerSettings _loggerSettings;
        private readonly IConsole _console;

        public ConsoleLogger(IConsoleLoggerSettings loggerSettings, IConsole console) : base(loggerSettings)
        {
            _loggerSettings = loggerSettings;
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public ConsoleColor GetColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Warning:
                    return _loggerSettings.WarningForeColor;
                case LogLevel.Error:
                    return _loggerSettings.ErrorForeColor;
                default:
                    return _loggerSettings.InfoForeColor;
            }
        }

        protected override Task WriteMessageAsync(string message, LogLevel logLevel)
        {
            ConsoleColor messageColor = GetColor(logLevel);
            _console.ForegroundColor = messageColor;

            _console.WriteLine(message);

            _console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
