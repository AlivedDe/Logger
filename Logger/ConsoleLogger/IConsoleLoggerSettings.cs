using System;

namespace Logger
{
    /// <summary>
    /// Settings for Console Logger include Colors
    /// </summary>
    public interface IConsoleLoggerSettings : ILoggerSettings
    {
        /// <summary>
        /// Foreground color for Error messages
        /// </summary>
        ConsoleColor ErrorForeColor { get; }

        /// <summary>
        /// Foreground color for Warning messages
        /// </summary>
        ConsoleColor WarningForeColor { get; }

        /// <summary>
        /// Foreground color for Info messages
        /// </summary>
        ConsoleColor InfoForeColor { get; }
    }
}
