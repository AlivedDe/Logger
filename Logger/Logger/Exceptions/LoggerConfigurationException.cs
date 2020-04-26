using System;

namespace Logger.Exceptions
{
    public class LoggerConfigurationException : Exception
    {
        public LoggerConfigurationException() : base("Invalid configuration")
        {

        }
    }
}
