namespace HostingNarrator.Abstracts
{
    public interface ILogger
    {
        void Log(string message, LogLevel logLevel);

        void LogError(string message);
    }
}