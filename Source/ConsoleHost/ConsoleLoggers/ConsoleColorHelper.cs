using HostingNarrator.Abstracts;
using System;

namespace ConsoleHost
{
    public static class ConsoleHelper
    {
        public static void SetConsoleColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Normal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case LogLevel.Detailed:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case LogLevel.All:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }
        }

        public static void ResetConsoleColor()
        {
            Console.ResetColor();
        }

        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.WriteLine(message);
            Console.WriteLine();

            Console.ResetColor();
        }
    }
}