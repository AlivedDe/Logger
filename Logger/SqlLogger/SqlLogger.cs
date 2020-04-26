using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Logger
{
    /// <summary>
    /// Inserts a message into Sql Table Log
    /// </summary>
    public class SqlLogger : AbstractLogger, ILogger
    {
        private readonly ISqlLoggerSettings _settings;
        private const string InsertCommand = "INSERT INTO Log VALUES(@param1, @param2)";

        public SqlLogger(ISqlLoggerSettings settings) : base(settings)
        {
            _settings = settings;
        }

        protected string GetLogLevelId(LogLevel logLevel)
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
            string logIndex = GetLogLevelId(logLevel);
            using (var connection = new SqlConnection(_settings.ConnectionString))
            {
                string formattedMessage = FormatMessage(message, logLevel);
                connection.Open();

                SqlCommand command = new SqlCommand(InsertCommand, connection);

                command.Parameters.AddWithValue("@param1", formattedMessage);
                command.Parameters.AddWithValue("@param2", logIndex);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}
