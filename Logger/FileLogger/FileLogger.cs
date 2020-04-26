using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class FileLogger : AbstractLogger, ILogger
    {
        private readonly IFileLoggerSettings _settings;
        private readonly IFileSystem _fileSystem;

        public FileLogger(IFileLoggerSettings settings, IFileSystem fileSystem) : base(settings)
        {
            _settings = settings;
            _fileSystem = fileSystem;
        }

        protected override async Task WriteMessageAsync(string message, LogLevel logLevel)
        {
            string fileName = Path.Combine(_settings.Directory, string.Format(_settings.FileName, DateTime.Now.ToShortDateString()));

            string formattedMessage = FormatMessage(message, logLevel);
            byte[] bytes = Encoding.UTF8.GetBytes(formattedMessage);

            using (StreamWriter fileStream = _fileSystem.AppendText(fileName))
            {
                await fileStream.WriteLineAsync(formattedMessage).ConfigureAwait(false);
            };
        }
    }
}
