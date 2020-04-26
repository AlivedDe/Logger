namespace Logger
{
    /// <summary>
    /// Settings for a file logger
    /// </summary>
    public interface IFileLoggerSettings : ILoggerSettings
    {
        /// <summary>
        /// File logger output directory
        /// </summary>
        string Directory { get; }

        /// <summary>
        /// File name format
        /// </summary>
        string FileName { get; }
    }
}
