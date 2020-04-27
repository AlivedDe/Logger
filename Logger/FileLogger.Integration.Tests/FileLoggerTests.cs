using Logger;
using Logger.IO;
using Logger.Settings;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FileLogger.Integration.Tests
{
    public class FileLoggerTests : IDisposable
    {
        private readonly DefaultFileLoggerSettings _settings;
        private readonly Logger.FileLogger _logger;

        public FileLoggerTests()
        {
            string expectedConfigurationPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            if (File.Exists(expectedConfigurationPath))
            {
                File.Delete(expectedConfigurationPath);
            }
            string actualConfigurationPath = Path.Combine(new FileInfo(expectedConfigurationPath).DirectoryName,
                "FileLogger.Integration.Tests.dll.config");
            File.Copy(actualConfigurationPath, expectedConfigurationPath);

            _settings = new DefaultFileLoggerSettings();
            _logger = new Logger.FileLogger(_settings, new FileSystemAdapter());
            if (Directory.Exists(_settings.Directory))
            {
                Directory.Delete(_settings.Directory, true);
            }
            Directory.CreateDirectory(_settings.Directory);
        }

        [Fact]
        public async Task LogMessageAsync_ShouldCreateFileAndPutRecord_WhenFileDoesNotExist()
        {
            //arrange
            var files = Directory.GetFiles(_settings.Directory);
            foreach(string file in files)
            {
                File.Delete(file);
            }
            DateTime now = DateTime.UtcNow;

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Error);

            //assert
            files = Directory.GetFiles(_settings.Directory);
            int filesCount = files.Length;
            Assert.Equal(1, filesCount);
            string logFile = files[0];
            string text = File.ReadAllText(logFile);
            Assert.Equal($"{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm:ss")}:\tTest\r\n", text);
        }

        [Fact]
        public async Task LogMessageAsync_ShouldAppendText_WhenFileExists()
        {
            //arrange
            var files = Directory.GetFiles(_settings.Directory);
            foreach (string file in files)
            {
                File.Delete(file);
            }
            string logFile = Path.Combine(_settings.Directory, string.Format(_settings.FileName, DateTime.UtcNow.ToString("yyyy-MM-dd")));
            using (var fileStream = File.Create(logFile))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    sw.Write("ExistingLine\r\n");
                }
            }
            DateTime now = DateTime.UtcNow;

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Error);

            //assert
            files = Directory.GetFiles(_settings.Directory);
            int filesCount = files.Length;
            Assert.Equal(1, filesCount);
            Assert.True(File.Exists(logFile));
            Assert.Equal(logFile, files[0]);
            string text = File.ReadAllText(logFile);
            Assert.Equal($"ExistingLine\r\n{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm:ss")}:\tTest\r\n", text);
        }

        public void Dispose()
        {
            Directory.Delete(_settings.Directory, true);
        }
    }
}
