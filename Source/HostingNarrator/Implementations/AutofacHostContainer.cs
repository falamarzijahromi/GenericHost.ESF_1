using Autofac;
using DynamicServiceHost.Host.Abstracts;
using HostingNarrator.Abstracts;
using System;

namespace HostingNarrator.Implementations
{
    public class AutofacHostContainer : IHostContainer
    {
        private AutofacIocContainer iocContainer;
        private IContainer container;
        private readonly ILogger logger;

        public AutofacHostContainer(AutofacIocContainer iocContainer, ILogger logger)
        {
            this.iocContainer = iocContainer;
            this.logger = logger;
        }

        public void SetRootContainerUp()
        {
            container = iocContainer.CreateContainer();
        }

        public IHostContainer CreateLifeScope()
        {
            var lifetimeScope = container.BeginLifetimeScope();

            var lifetimeScopeContainer = new AutoFacLifetimeScope(lifetimeScope, logger);

            logger.Log("LifetimeScope Created", LogLevel.All);

            return lifetimeScopeContainer;
        }

        public void Dispose()
        {
            iocContainer.Dispose();

            logger.Log("The origin container disposed", LogLevel.All);
        }

        public void RegisterSingleton<T>(T instance)
        {
            iocContainer.RegisterSingleton<T>(instance);

            logger.Log($"Host container registered {instance.GetType().FullName} as singleton for {typeof(T).FullName}", LogLevel.All);
        }

        public void RegisterTransient(Type from, Type to)
        {
            iocContainer.RegisterTransient(from, to);

            logger.Log($"Host container registered service type: {from.FullName} as  {to.FullName}", LogLevel.All);
        }

        public object Resolve(Type type)
        {
            throw new NotSupportedException("Only LifetimeScopes can resolve.");
        }
    }
}