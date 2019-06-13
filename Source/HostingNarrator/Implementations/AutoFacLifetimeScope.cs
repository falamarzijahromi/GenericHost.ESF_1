using System;
using Autofac;
using DynamicServiceHost.Host.Abstracts;
using HostingNarrator.Abstracts;

namespace HostingNarrator.Implementations
{
    public class AutoFacLifetimeScope : IHostContainer
    {
        private readonly ILifetimeScope lifetimeScope;
        private readonly ILogger logger;

        public AutoFacLifetimeScope(
            ILifetimeScope lifetimeScope, 
            ILogger logger)
        {
            this.lifetimeScope = lifetimeScope;
            this.logger = logger;
        }

        public IHostContainer CreateLifeScope()
        {
            throw new NotSupportedException();
        }

        public void Dispose()
        {
            lifetimeScope.Dispose();
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
            try
            {
                return lifetimeScope.Resolve(type);
            }
            catch (Exception e)
            {
                logger.LogError($"Resolving {type.FullName} ended with exception:{Environment.NewLine}{e.Message}");

                throw;
            }
        }
    }
}