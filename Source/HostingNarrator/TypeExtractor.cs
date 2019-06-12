using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HostingNarrator.Abstracts;

namespace HostingNarrator
{
    public class TypeExtractor
    {
        internal static List<Type> Extract(
            Assembly contractAssembly,
            string nameContainee,
            ILogger logger)
        {
            var extractedTypes = contractAssembly
                .ExportedTypes
                .Where(type => type.FullName.Contains(nameContainee) && type.IsInterface)
                .ToList();

            var extractedTypeNames = extractedTypes.Aggregate(
                new StringBuilder(), 
                (seed, type) => seed.Append(type.FullName + Environment.NewLine));

            logger.Log($"Following types extracted for {nameContainee} : {Environment.NewLine}{extractedTypeNames.ToString()}", LogLevel.Detailed);

            return extractedTypes;
        }
    }
}