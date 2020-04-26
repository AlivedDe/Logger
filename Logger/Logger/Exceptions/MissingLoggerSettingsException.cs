using System;

namespace Logger.Exceptions
{
    public class MissingLoggerSettingsException : Exception
    {
        public MissingLoggerSettingsException() : base("Logger settings are requried")
        {

        }
    }
}
