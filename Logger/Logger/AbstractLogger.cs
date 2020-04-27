using Logger.Exceptions;
using System;
using System.Threading.Tasks;

namespace Logger
{
    public abstract class AbstractLogger : ILogger
    {
        private ILoggerSettings _loggerSettings;

        public AbstractLogger(ILoggerSettings loggerSettings)
        {
            _loggerSettings = loggerSettings ?? throw new MissingLoggerSettingsException();
        }

        public Task LogMessageAsync(string message, LogLevel logLevel)
        {
            if (!ShouldLog(message) || !ShouldLog(logLevel))
            {
                return Task.CompletedTask;
            }

            string formattedMessage = FormatMessage(message, logLevel);
            return WriteMessageAsync(formattedMessage, logLevel);
        }

        public bool ShouldLog(LogLevel logLevel)
        {
            return _loggerSettings.LogLevel >= logLevel;
        }

        public bool ShouldLog(string message)
        {
            return !string.IsNullOrWhiteSpace(message);
        }

        public string FormatMessage(string message, LogLevel logLevel)
        {
            return string.Format(_loggerSettings.MessageFormat, logLevel, DateTime.UtcNow, message);
        }

        protected abstract Task WriteMessageAsync(string message, LogLevel logLevel);
    }
}
