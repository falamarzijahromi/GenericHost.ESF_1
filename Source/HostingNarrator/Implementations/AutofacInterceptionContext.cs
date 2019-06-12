using System;
using Castle.DynamicProxy;
using Composition.ESF_1;

namespace HostingNarrator.Implementations
{
    public class AutofacInterceptionContext : IInterceptionContext
    {
        private readonly IInvocation invokationContext;

        public AutofacInterceptionContext(IInvocation invokationContext)
        {
            this.invokationContext = invokationContext;
        }

        public Type TargetType => invokationContext.TargetType;

        public string MethodName => invokationContext.Method.Name;

        public void Proceed()
        {
            invokationContext.Proceed();
        }
    }
}