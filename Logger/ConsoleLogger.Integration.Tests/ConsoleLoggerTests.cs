using Logger;
using Logger.Console;
using Logger.Settings;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleLogger.Integration.Tests
{
    public class ConsoleLoggerTests
    {
        private readonly Logger.ConsoleLogger _logger;

        public ConsoleLoggerTests()
        {
            var settings = new DefaultConsoleLoggerSettings();
            _logger = new Logger.ConsoleLogger(settings, new ConsoleAdapter());
        }

        [Fact]
        public async Task LogMessageAsync_ShouldWriteIntoConsole_WhenProperMEssageAndLogLevel()
        {
            //arrange
            string result;
            DateTime now;

            using (TextWriter writer = new StringWriter())
            {
                Console.SetOut(writer);
                Console.SetError(writer);
                now = DateTime.UtcNow;

                //act
                await _logger.LogMessageAsync("Test", LogLevel.Error);
                result = writer.ToString();
            }

            //assert
            Assert.Equal($"{LogLevel.Error}\t{now.ToString("yyyy-MM-dd_HH:mm:ss")}:\tTest\r\n", result);
        }
    }
}
