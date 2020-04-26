namespace Logger
{
    /// <summary>
    /// Sql Logger settings
    /// </summary>
    public interface ISqlLoggerSettings : ILoggerSettings
    {
        /// <summary>
        /// Connection string to a logger database
        /// </summary>
        string ConnectionString { get; }
    }
}