namespace Logger
{
    public interface ILoggerSettings
    {
        LogLevel LogLevel { get; }
        string MessageFormat { get; }
    }
}
