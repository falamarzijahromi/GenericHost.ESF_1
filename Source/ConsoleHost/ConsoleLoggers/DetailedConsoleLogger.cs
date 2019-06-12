using System;
using HostingNarrator.Abstracts;

namespace ConsoleHost
{
    public class DetailedConsoleLogger : ILogger
    {

        public void Log(string message, LogLevel logLevel)
        {
            if (logLevel == LogLevel.Detailed || logLevel == LogLevel.Normal)
            {
                ConsoleHelper.SetConsoleColor(logLevel);

                Console.WriteLine(message);
                Console.WriteLine();

                ConsoleHelper.ResetConsoleColor();
            }
        }

        public void LogError(string message)
        {
            ConsoleHelper.LogError(message);
        }
    }
}