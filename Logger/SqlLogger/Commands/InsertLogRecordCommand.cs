using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Logger.Commands
{
    public class InsertLogRecordCommand : IInsertLogRecordCommand
    {
        private const string InsertCommand = "INSERT INTO Log VALUES(@param1, @param2)";

        public async Task Insert(string connectionString, string message, string levelId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(InsertCommand, connection);

                command.Parameters.AddWithValue("@param1", message);
                command.Parameters.AddWithValue("@param2", levelId);
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}
