using System.Threading.Tasks;

namespace Logger.Tests.Helpers
{
    public interface ILogWriter
    {
        Task WriteMessageAsync(string message, LogLevel logLevel);
    }
}
