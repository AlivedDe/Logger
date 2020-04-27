using System;
using System.Threading.Tasks;

namespace Logger
{
    /// <summary>
    /// Inserts a message into Sql Table Log
    /// </summary>
    public class SqlLogger : AbstractLogger, ILogger
    {
        private readonly ISqlLoggerSettings _settings;
        private readonly IInsertLogRecordCommand _insertCommand;

        public SqlLogger(ISqlLoggerSettings settings, IInsertLogRecordCommand insertCommand) : base(settings)
        {
            _settings = settings;
            _insertCommand = insertCommand ?? throw new ArgumentNullException(nameof(insertCommand));
        }

        public string GetLogLevelId(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    return "2";
                case LogLevel.Warning:
                    return "3";
                case LogLevel.Info:
                    return "1";
                default:
                    return "0";
            }
        }

        protected override async Task WriteMessageAsync(string message, LogLevel logLevel)
        {
            string logLevelId = GetLogLevelId(logLevel);
            await _insertCommand.Insert(_settings.ConnectionString, message, logLevelId);
        }
    }
}
