using System;
using System.IO;
using HostingNarrator.Abstracts;

namespace WindowsServiceHost
{
    public class NotepadLogger : ILogger, IDisposable
    {
        private readonly StreamWriter writer;

        public NotepadLogger(string logPath)
        {
            writer = new StreamWriter(logPath, true);
        }

        public void Log(string message, LogLevel logLevel)
        {
            var logMessage = $"({logLevel})-({DateTime.Now})-{message}";

            writer.WriteLine(logMessage);
        }

        public void LogError(string message)
        {
            var logMessage = $"(Error)-({DateTime.Now})-{message}";

            writer.WriteLine(logMessage);
        }

        public void Dispose()
        {
            writer?.Dispose();
        }
    }
}
