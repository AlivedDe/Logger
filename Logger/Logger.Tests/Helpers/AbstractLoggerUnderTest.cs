using System.Threading.Tasks;

namespace Logger.Tests.Helpers
{
    internal class AbstractLoggerUnderTest : AbstractLogger
    {
        private readonly ILogWriter _writer;

        public AbstractLoggerUnderTest(ILoggerSettings settings, ILogWriter writer) : base(settings)
        {
            _writer = writer;
        }

        protected override Task WriteMessageAsync(string message, LogLevel logLevel)
        {
            return _writer.WriteMessageAsync(message, logLevel);
        }
    }
}
