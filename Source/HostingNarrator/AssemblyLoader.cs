using HostingNarrator.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HostingNarrator
{
    public class AssemblyLoader
    {
        public static List<Assembly> LoadAllAssemblies(DirectoryInfo directory, ILogger logger)
        {
            var dlls =
                from file in directory.GetFiles().AsQueryable()
                where file.Extension.Contains("dll") == true
                select Assembly.LoadFrom(file.FullName);

            if (!dlls.Any())
            {
                throw new Exception($"No dll found in directory: {directory.FullName}");
            }

            var dllNames = dlls.Aggregate(
                new StringBuilder(),
                (seed, dll) => seed.Append(dll.FullName + Environment.NewLine));

            logger.Log($"Following dlls found in directory: {directory.FullName} :{Environment.NewLine}{dllNames.ToString()}", LogLevel.Detailed);

            return dlls.ToList();
        }
    }
}