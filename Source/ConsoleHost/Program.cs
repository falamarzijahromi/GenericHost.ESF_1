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
            var bcDirInput = ExtractArg(args, 0);
            var logLevelInput = ExtractArg(args, 1);

            var logger = GetLogger(logLevelInput);

            var directory = GetDirectory(bcDirInput);

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

        private static string ExtractArg(string[] args, int index)
        {
            try
            {
                return args[index];
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private static DirectoryInfo GetDirectory(string bcDir = null)
        {
            bcDir = bcDir ?? ConfigurationManager.AppSettings["BcDir"];

            var dir = new DirectoryInfo(bcDir);

            return dir;
        }

        private static ILogger GetLogger(string logLevelString = null)
        {
            logLevelString = logLevelString ?? ConfigurationManager.AppSettings["LogLevel"];

            var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevelString);

            var logger = ConsoleLoggerFactory.CreateLogger(logLevel);

            return logger;
        }
    }
}
