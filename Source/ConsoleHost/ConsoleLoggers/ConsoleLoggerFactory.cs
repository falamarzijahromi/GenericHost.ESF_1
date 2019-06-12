using HostingNarrator.Abstracts;

namespace ConsoleHost
{
    public static class ConsoleLoggerFactory
    {
        public static ILogger CreateLogger(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Normal:
                    return new NormalConsoleLogger();

                case LogLevel.Detailed:
                    return new DetailedConsoleLogger();

                case LogLevel.All:
                    return new AllConsoleLogger();

                default:
                    return new NormalConsoleLogger();
            }
        }
    }
}