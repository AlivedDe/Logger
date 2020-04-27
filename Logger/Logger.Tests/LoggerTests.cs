using Logger.Exceptions;
using Logger.Tests.Helpers;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Logger.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void Ctor_ShouldThrowException_WhenLoggersNotProvided()
        {
            //act
            Assert.Throws<LoggerConfigurationException>(() => new Logger());
        }

        [Fact]
        public async Task LogMessage_ShouldCallAllLoggers_WhenCalled()
        {
            //arrange
            const string message = "Test";
            const LogLevel logLevel = LogLevel.Error;

            var logger1Parameter = Substitute.For<ILogger>();
            logger1Parameter.LogMessageAsync(message, logLevel).Returns(Task.CompletedTask);
            var logger2Parameter = Substitute.For<ILogger>();
            logger2Parameter.LogMessageAsync(message, logLevel).Returns(Task.CompletedTask);
            var logger = new Logger(logger1Parameter, logger2Parameter);

            //act
            await logger.LogMessageAsync(message, logLevel);

            //assert
            await logger1Parameter.Received(1).LogMessageAsync(message, logLevel);
            await logger2Parameter.Received(1).LogMessageAsync(message, logLevel);
        }
    }
}
