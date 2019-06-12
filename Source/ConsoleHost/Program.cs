using HostingNarrator;
using HostingNarrator.Abstracts;
using System;
using System.Configuration;
using System.IO;

namespace ConsoleHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var logger = GetLogger();

            var directory = GetDirectory();

            var narrator = new Narrator(directory, logger);

            Console.WriteLine("Press enter to start hosting bounded context");
            Console.WriteLine();
            Console.ReadLine();

            narrator.Start();

            while (true)
            {
                var command = Console.ReadLine();

                if (command == "Exit Hosting")
                {
                    break;
                }
            }
        }

        private static DirectoryInfo GetDirectory()
        {
            var bcDir = ConfigurationManager.AppSettings["BcDir"];

            var dir = new DirectoryInfo(bcDir);

            return dir;
        }

        private static ILogger GetLogger()
        {
            var logLevelString = ConfigurationManager.AppSettings["LogLevel"];

            var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevelString);

            var logger = ConsoleLoggerFactory.CreateLogger(logLevel);

            return logger;
        }
    }
}
