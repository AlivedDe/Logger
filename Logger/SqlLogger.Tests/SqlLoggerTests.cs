using Logger;
using Logger.Exceptions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SqlLogger.Tests
{
    public class SqlLoggerTests
    {
        private readonly ISqlLoggerSettings _settings;
        private readonly IInsertLogRecordCommand _insertCommand;
        private readonly Logger.SqlLogger _logger;

        public SqlLoggerTests()
        {
            _settings = Substitute.For<ISqlLoggerSettings>();
            _settings.LogLevel.Returns(LogLevel.Info);
            _settings.ConnectionString.Returns("Test");
            _settings.MessageFormat.Returns("{0}\t{1:yyyy-MM-dd_HH:mm}:\t{2}");
            _insertCommand = Substitute.For<IInsertLogRecordCommand>();
            _insertCommand.Insert(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);
            _logger = new Logger.SqlLogger(_settings, _insertCommand);
        }

        [Fact]
        public void Ctor_ShouldThrowException_WhenSettingsAreNull()
        {
            Assert.Throws<MissingLoggerSettingsException>(() => new Logger.SqlLogger(null, Substitute.For<IInsertLogRecordCommand>()));
        }

        [Fact]
        public void Ctor_ShouldThrowException_WhenConsoleAdapterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Logger.SqlLogger(Substitute.For<ISqlLoggerSettings>(), null));
        }

        [Fact]
        public async Task LogMessageAsync_ShouldNotLog_WhenEmptyMessage()
        {
            //act
            await _logger.LogMessageAsync("", LogLevel.Error);

            //assert
            await _insertCommand.DidNotReceiveWithAnyArgs().Insert(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task LogMessageAsync_ShouldNotLog_WhenLogLevelInvalid()
        {
            //arrange
            _settings.LogLevel.Returns(LogLevel.Error);

            //act
            await _logger.LogMessageAsync("Test", LogLevel.Warning);

            //assert
            await _insertCommand.DidNotReceiveWithAnyArgs().Insert(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void GetLogLevelId_ShouldReturn2_WhenError()
        {
            //act
            string result = _logger.GetLogLevelId(LogLevel.Error);

            //assert
            Assert.Equal("2", result);
        }

        [Fact]
        public void GetLogLevelId_ShouldReturn3_WhenWarning()
        {
            //act
            string result = _logger.GetLogLevelId(LogLevel.Warning);

            //assert
            Assert.Equal("3", result);
        }

        [Fact]
        public void GetLogLevelId_ShouldReturn1_WhenInfo()
        {
            //act
            string result = _logger.GetLogLevelId(LogLevel.Info);

            //assert
            Assert.Equal("1", result);
        }

        [Theory]
        [InlineData((LogLevel)0)]
        [InlineData((LogLevel)3)]
        [InlineData((LogLevel)5)]
        public void GetLogLevelId_ShouldReturn0_WhenUnknownLevel(LogLevel logLevel)
        {
            //act
            string result = _logger.GetLogLevelId(logLevel);

            //assert
            Assert.Equal("0", result);
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
            await _insertCommand.Received(1).Insert(_settings.ConnectionString,
                $"{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm")}:\tTest",
                "2");
        }
    }
}
