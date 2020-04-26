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
        private static readonly object _locker = new object();

        public ConsoleLogger(IConsoleLoggerSettings loggerSettings) : base(loggerSettings)
        {
            _loggerSettings = loggerSettings;
        }

        protected override Task WriteMessageAsync(string message, LogLevel logLevel)
        {
            return Task.Run(() =>
            {
                lock (_locker)
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    ConsoleColor messageColor = _loggerSettings.GetColor(logLevel);
                    Console.ForegroundColor = messageColor;

                    Console.WriteLine(FormatMessage(message, logLevel));

                    Console.ForegroundColor = originalColor;
                }
            });
        }
    }
}
