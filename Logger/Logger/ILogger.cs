using System;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {
        Task LogMessageAsync(string message, LogLevel logLevel);

        bool ShouldLog(LogLevel logLevel);

        bool ShouldLog(string message);
    }
}
