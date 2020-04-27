using System;
using System.IO;
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
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        protected override async Task WriteMessageAsync(string message, LogLevel logLevel)
        {
            string fileName = Path.Combine(_settings.Directory, string.Format(_settings.FileName, DateTime.UtcNow.ToString("yyyy-MM-dd")));

            await _fileSystem.AppendFileAsync(fileName, message);
        }
    }
}
