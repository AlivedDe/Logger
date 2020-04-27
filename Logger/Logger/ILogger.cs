using System;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {
        Task LogMessageAsync(string message, LogLevel logLevel);
    }
}
