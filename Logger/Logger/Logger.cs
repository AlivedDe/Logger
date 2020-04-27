using Logger.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Logger
{
    public class Logger : ILogger
    {
        private readonly ILogger[] _loggers;

        public Logger(params ILogger[] loggers)
        {
            if (loggers.Length == 0)
            {
                throw new LoggerConfigurationException();
            }
            _loggers = loggers;
        }

        public Task LogMessageAsync(string message, LogLevel logLevel)
        {
            return Task.WhenAll(_loggers.Select(logger => logger.LogMessageAsync(message, logLevel)));
        }
    }
}
