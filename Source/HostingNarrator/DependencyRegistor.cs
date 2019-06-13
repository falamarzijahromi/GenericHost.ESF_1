using DynamicServiceHost.Host.Abstracts;
using System;
using HostingNarrator.Abstracts;
using HostingNarrator.Implementations;

namespace HostingNarrator
{
    public class DependencyRegistor
    {
        public static IHostContainer RegisterDependencies(Type compositionRootType, ILogger logger, out Action containerSetup)
        {
            var iocContainer = new AutofacIocContainer(logger);

            var method = compositionRootType.GetMethod("RegisterDependecies");

            if (method == null)
            {
                throw new Exception($"RegisterDependecies method not found in type: {compositionRootType.FullName}");
            }

            logger.Log($"RegisterDependecies method found in type: {compositionRootType.FullName}", LogLevel.Detailed);

            method.Invoke(null, new[] {iocContainer});

            var hostContainer = new AutofacHostContainer(iocContainer, logger);

            hostContainer.RegisterSingleton<ILogger>(logger);

            containerSetup = () => hostContainer.SetRootContainerUp();

            return hostContainer;
        }
    }
}