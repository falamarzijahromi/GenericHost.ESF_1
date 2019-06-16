using System;
using Autofac;
using Composition.ESF_1;

namespace HostingNarrator.Implementations
{
    public class AutofactResolver : IResolver
    {
        private readonly IComponentContext componentContext;

        public AutofactResolver(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public object Resolver(Type type)
        {
            return componentContext.Resolve(type);
        }
    }
}