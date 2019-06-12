using System;
using HostingNarrator.Abstracts;

namespace ConsoleHost
{
    public class NormalConsoleLogger : ILogger
    {
        public void Log(string message, LogLevel logLevel)
        {
            if (logLevel == LogLevel.Normal)
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