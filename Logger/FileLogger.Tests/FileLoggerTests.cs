using Logger;
using Logger.Exceptions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FileLogger.Tests
{
    public class FileLoggerTests
    {
        private readonly IFileLoggerSettings _settings;
        private readonly IFileSystem _fileSystem;
        private readonly Logger.FileLogger _logger;

        public FileLoggerTests()
        {
            _settings = Substitute.For<IFileLoggerSettings>();
            _settings.LogLevel.Returns(LogLevel.Info);
            _settings.Directory.Returns("C:\\Test");
            _settings.FileName.Returns("Log_{0}.txt");
            _settings.MessageFormat.Returns("{0}\t{1:yyyy-MM-dd_HH:mm}:\t{2}");
            _fileSystem = Substitute.For<IFileSystem>();
            _fileSystem.AppendFileAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);
            _logger = new Logger.FileLogger(_settings, _fileSystem);
        }

        [Fact]
        public void Ctor_ShouldThrowException_WhenSettingsAreNull()
        {
            Assert.Throws<MissingLoggerSettingsException>(() => new Logger.FileLogger(null, Substitute.For<IFileSystem>()));
        }

        [Fact]
        public void Ctor_ShouldThrowException_WhenConsoleAdapterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Logger.FileLogger(Substitute.For<IFileLoggerSettings>(), null));
        }

        [Fact]
        public async Task LogMessageAsync_ShouldNotLog_WhenEmptyMessage()
        {
            //act
            await _logger.LogMessageAsync("", LogLevel.Error);

            //assert
            await _fileSystem.DidNotReceiveWithAnyArgs().AppendFileAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task LogMessageAsync_ShouldNotLog_WhenLogLevelInvalid()
        {
            //arrange
            _settings.LogLevel.Returns(LogLevel.Error);

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Warning);

            //assert
            await _fileSystem.DidNotReceiveWithAnyArgs().AppendFileAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task LogMessageAsync_ShouldWriteMessageWithProperColor_WhenProperMessageAndLogLevel()
        {
            //arrange
            _settings.LogLevel.Returns(LogLevel.Error);
            DateTime now = DateTime.UtcNow;

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Error);

            //assert
            await _fileSystem.Received(1).AppendFileAsync($"{_settings.Directory}\\Log_{now.ToShortDateString()}.txt",
                $"{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm")}:\tTest");
        }
    }
}
