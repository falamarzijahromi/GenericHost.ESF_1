using System;

namespace HostingNarrator.Abstracts
{
    [Flags]
    public enum LogLevel
    {
        Normal = 2,
        Detailed = 4,
        All = 8,
    }
}