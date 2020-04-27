using System.Threading.Tasks;

namespace Logger
{
    public interface IInsertLogRecordCommand
    {
        Task Insert(string connectionString, string message, string levelId);
    }
}
