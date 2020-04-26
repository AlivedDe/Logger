using Logger.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Logger
{
    public class Logger : AbstractLogger, ILogger
    {
        private readonly ILogger[] _loggers;

        public Logger(ILoggerSettings loggerSettings, params ILogger[] loggers) : base(loggerSettings)
        {
            if (loggers.Length == 0)
            {
                throw new LoggerConfigurationException();
            }
            _loggers = loggers;
        }

        protected override Task WriteMessageAsync(string message, LogLevel logLevel)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return Task.CompletedTask;
            }

            return Task.WhenAll(_loggers.Select(logger => logger.LogMessageAsync(message, logLevel)));
        }
    }
}
