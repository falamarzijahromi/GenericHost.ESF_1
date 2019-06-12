using System;
using Autofac;
using DynamicServiceHost.Host.Abstracts;
using HostingNarrator.Abstracts;

namespace HostingNarrator.Implementations
{
    public class AutoFacLifetimeScope : IHostContainer
    {
        private readonly ILifetimeScope lifetimeScope;
        private readonly IContainer container;
        private readonly ILogger logger;

        public AutoFacLifetimeScope(
            ILifetimeScope lifetimeScope, 
            IContainer container, 
            ILogger logger)
        {
            this.lifetimeScope = lifetimeScope;
            this.container = container;
            this.logger = logger;
        }

        public IHostContainer CreateLifeScope()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            lifetimeScope.Dispose();
            container.Dispose();
        }

        public void RegisterSingleton<T>(T instance)
        {
            throw new NotSupportedException();
        }

        public void RegisterTransient(Type from, Type to)
        {
            throw new NotSupportedException();
        }

        public object Resolve(Type type)
        {
            return lifetimeScope.Resolve(type);
        }
    }
}