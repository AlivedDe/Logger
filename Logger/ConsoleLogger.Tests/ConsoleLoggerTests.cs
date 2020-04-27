using Logger;
using Logger.Exceptions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleLogger.Tests
{
    public class ConsoleLoggerTests
    {
        private readonly IConsoleLoggerSettings _settings;
        private readonly IConsole _console;
        private readonly Logger.ConsoleLogger _logger;

        public ConsoleLoggerTests()
        {
            _settings = Substitute.For<IConsoleLoggerSettings>();
            _settings.LogLevel.Returns(LogLevel.Info);
            _settings.InfoForeColor.Returns(ConsoleColor.Blue);
            _settings.WarningForeColor.Returns(ConsoleColor.Yellow);
            _settings.ErrorForeColor.Returns(ConsoleColor.Red);
            _settings.MessageFormat.Returns("{0}\t{1:yyyy-MM-dd_HH:mm}:\t{2}");
            _console = Substitute.For<IConsole>();
            _logger = new Logger.ConsoleLogger(_settings, _console);
        }

        [Fact]
        public void Ctor_ShouldThrowException_WhenSettingsAreNull()
        {
            Assert.Throws<MissingLoggerSettingsException>(() => new Logger.ConsoleLogger(null, Substitute.For<IConsole>()));
        }

        [Fact]
        public void Ctor_ShouldThrowException_WhenConsoleAdapterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Logger.ConsoleLogger(Substitute.For<IConsoleLoggerSettings>(), null));
        }

        [Fact]
        public void GetColor_ShouldReturnErrorForeground_WhenLevelIsError()
        {
            //act
            ConsoleColor result = _logger.GetColor(LogLevel.Error);

            //assert
            Assert.Equal(_settings.ErrorForeColor, result);
        }

        [Fact]
        public void GetColor_ShouldReturnWarningForeground_WhenLevelIsWarning()
        {
            //act
            ConsoleColor result = _logger.GetColor(LogLevel.Warning);

            //assert
            Assert.Equal(_settings.WarningForeColor, result);
        }

        [Theory]
        [InlineData(LogLevel.Info)]
        [InlineData((LogLevel)5)]
        public void GetColor_ShouldReturnInfoForeground_WhenLevelIsNotErrorOrWarning(LogLevel logLevel)
        {
            //act
            ConsoleColor result = _logger.GetColor(logLevel);

            //assert
            Assert.Equal(_settings.InfoForeColor, result);
        }

        [Fact]
        public async Task LogMessageAsync_ShouldNotLog_WhenEmptyMessage()
        {
            //act
            await _logger.LogMessageAsync("", LogLevel.Error);

            //assert
            _console.DidNotReceiveWithAnyArgs().WriteLine(Arg.Any<string>());
        }

        [Fact]
        public async Task LogMessageAsync_ShouldNotLog_WhenLogLevelInvalid()
        {
            //arrange
            _settings.LogLevel.Returns(LogLevel.Error);

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Warning);

            //assert
            _console.DidNotReceiveWithAnyArgs().WriteLine(Arg.Any<string>());
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
            _console.Received(1).ForegroundColor = _settings.ErrorForeColor;
            _console.Received(1).ResetColor();
            _console.Received(1).WriteLine($"{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm")}:\tTest");
        }
    }
}
