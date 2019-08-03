using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HostingNarrator.Abstracts;

namespace HostingNarrator
{
    public class Narrator : IDisposable
    {
        private readonly DirectoryInfo boundedContextDir;
        private readonly ModuleBuilder moduleBuilder;
        private readonly ILogger logger;

        private ServiceHost commandServiceHost;
        private ServiceHost queryServiceHost;

        public Narrator(DirectoryInfo boundedContextDir, ILogger logger)
        {
            this.boundedContextDir = boundedContextDir;
            this.logger = logger;

            var asmName = new AssemblyName("SomeName");

            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);

            moduleBuilder = asmBuilder.DefineDynamicModule("DynamicModule");
        }

        public void Start()
        {
            try
            {
                logger.Log($"Narration started for directory: {boundedContextDir.FullName}", LogLevel.Normal);

                var dlls = AssemblyLoader.LoadAllAssemblies(boundedContextDir, logger);

                var compositionType = GetCompositionDll(dlls, logger);

                var container = DependencyRegistor.RegisterDependencies(compositionType, logger, out Action containerSetup);

                var contractAssembly = GetContractAssembly(dlls, logger);

                var commandTypes = TypeExtractor.Extract(contractAssembly, "Command", logger);
                var queryTypes = TypeExtractor.Extract(contractAssembly, "Query", logger);

                commandServiceHost = ServiceHost.HostCommands(commandTypes, moduleBuilder, container, logger);
                queryServiceHost = ServiceHost.HostQueries(queryTypes, moduleBuilder, container, logger);

                containerSetup.Invoke();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        private Assembly GetContractAssembly(List<Assembly> dlls, ILogger logger)
        {
            var contractDll = dlls.SingleOrDefault(dll => dll.FullName.Contains("Contract"));

            if (contractDll == null)
            {
                throw new Exception($"Contract assembly not found in directory: {boundedContextDir.FullName}");
            }

            logger.Log($"Contract assembly found in directory: {boundedContextDir.FullName}", LogLevel.Detailed);

            return contractDll;
        }

        private Type GetCompositionDll(List<Assembly> dlls, ILogger logger)
        {
            var compostionDll = dlls.SingleOrDefault(dll => dll.FullName.Contains("Composition"));

            if (compostionDll == null)
            {
                throw new Exception($"Composition root not found in directory: {boundedContextDir.FullName}");
            }

            logger.Log($"{compostionDll.FullName} found in directory: {boundedContextDir.FullName}", LogLevel.Detailed);

            var compositionRoot = compostionDll.ExportedTypes.SingleOrDefault(type => type.FullName.Contains("CompositionRoot"));

            if (compositionRoot == null)
            {
                throw new Exception($"CompositionRoot type not found in assembly: {compostionDll.FullName}");
            }

            logger.Log($"{compositionRoot.FullName} found in assembly: {compostionDll.FullName}", LogLevel.Detailed);

            return compositionRoot;
        }

        public void Dispose()
        {
            commandServiceHost?.Dispose();
            queryServiceHost?.Dispose();
        }
    }
}
