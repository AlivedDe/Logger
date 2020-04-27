using Logger.Exceptions;
using Logger.Tests.Helpers;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Logger.Tests
{

    public class AbstractLoggerTests
    {
        private readonly ILoggerSettings _loggerSettings;
        private readonly ILogWriter _logWriter;
        private readonly AbstractLoggerUnderTest _logger;

        public AbstractLoggerTests()
        {
            _loggerSettings = Substitute.For<ILoggerSettings>();
            _loggerSettings.MessageFormat.Returns("{0}\t{1:yyyy-MM-dd_HH:mm}:\t{2}");
            _logWriter = Substitute.For<ILogWriter>();
            _logWriter.WriteMessageAsync(Arg.Any<string>(), Arg.Any<LogLevel>()).Returns(Task.CompletedTask);
            _logger = new AbstractLoggerUnderTest(_loggerSettings, _logWriter);
        }

        [Fact]
        public void Ctor_SHouldThrowException_WhenSettingsAreNull()
        {
            //act
            Assert.Throws<MissingLoggerSettingsException>(() => new AbstractLoggerUnderTest(null, Substitute.For<ILogWriter>()));
        }

        [Theory]
        [InlineData(LogLevel.Error, LogLevel.Error)]
        [InlineData(LogLevel.Warning, LogLevel.Error)]
        [InlineData(LogLevel.Info, LogLevel.Error)]
        [InlineData(LogLevel.Warning, LogLevel.Warning)]
        [InlineData(LogLevel.Info, LogLevel.Warning)]
        [InlineData(LogLevel.Info, LogLevel.Info)]
        public void ShouldLog_ShouldReturnTrue_WhenConfiguredLevelIsGreaterThanOrEqualToMessageLevel(LogLevel configured, LogLevel messageLevel)
        {
            //arrange
            _loggerSettings.LogLevel.Returns(configured);

            //act
            bool result = _logger.ShouldLog(messageLevel);

            //assert
            Assert.True(result, $"Message should be logged when Logger is configured for {configured} and Message is {messageLevel}");
        }

        [Theory]
        [InlineData(LogLevel.Error, LogLevel.Warning)]
        [InlineData(LogLevel.Error, LogLevel.Info)]
        [InlineData(LogLevel.Warning, LogLevel.Info)]
        public void ShouldLog_ShouldReturnFalse_WhenConfiguredLevelIsLowerThanMessageLevel(LogLevel configured, LogLevel messageLevel)
        {
            //arrange
            _loggerSettings.LogLevel.Returns(configured);

            //act
            bool result = _logger.ShouldLog(messageLevel);

            //assert
            Assert.False(result, $"Message should NOT be logged when Logger is configured for {configured} and Message is {messageLevel}");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void ShouldLog_ShouldReturnFalse_WhenStringIsNullOrEmpty(string message)
        {
            //act
            bool result = _logger.ShouldLog(message);

            //assert
            Assert.False(result, $"Message should NOT be logged when empty message");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("3434324")]
        [InlineData(" 4343")]
        [InlineData("434 ")]
        public void ShouldLog_ShouldReturnTrue_WhenStringIsNotEmpty(string message)
        {
            //act
            bool result = _logger.ShouldLog(message);

            //assert
            Assert.True(result, $"Message should be logged when message is not empty");
        }

        [Fact]
        public void FormatMessage_ShouldReturnFormattedMessage_WhenMessageProvided()
        {
            //arrange
            DateTime now = DateTime.UtcNow;

            //act
            string result = _logger.FormatMessage("Test", LogLevel.Error);

            //assert
            Assert.Equal(result, $"{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm")}:\tTest");
        }

        [Fact]
        public async Task LogMessage_ShouldCallWriterMessageWithFormattedMessage_WhenShouldLogReturnsTrue()
        {
            //arrange
            _loggerSettings.LogLevel.Returns(LogLevel.Error);
            DateTime now = DateTime.UtcNow;

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Error);

            //arrange
            await _logWriter.Received(1).WriteMessageAsync($"{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm")}:\tTest", LogLevel.Error);
        }

        [Fact]
        public async Task LogMessage_ShouldNotCallWriterMessage_WhenMessageIsEmpty()
        {
            //arrange
            _loggerSettings.LogLevel.Returns(LogLevel.Error);
            DateTime now = DateTime.UtcNow;

            //act
            await _logger.LogMessageAsync("", LogLevel.Error);

            //arrange
            await _logWriter.DidNotReceiveWithAnyArgs().WriteMessageAsync(Arg.Any<string>(), Arg.Any<LogLevel>());
        }

        [Fact]
        public async Task LogMessage_ShouldNotCallWriterMessage_WhenMessageLevelIsGreaterThanConfigured()
        {
            //arrange
            _loggerSettings.LogLevel.Returns(LogLevel.Error);
            DateTime now = DateTime.UtcNow;

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Warning);

            //arrange
            await _logWriter.DidNotReceiveWithAnyArgs().WriteMessageAsync(Arg.Any<string>(), Arg.Any<LogLevel>());
        }
    }
}
