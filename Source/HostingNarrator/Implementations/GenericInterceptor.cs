using Castle.DynamicProxy;
using Composition.ESF_1;
using HostingNarrator.Abstracts;

namespace HostingNarrator.Implementations
{
    public class GenericInterceptor<T> : IGenericInterceptor<T> 
        where T : IServiceInterceptor
    {
        private readonly T serviceInterceptor;

        public GenericInterceptor(T serviceInterceptor)
        {
            this.serviceInterceptor = serviceInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
           var interceptionContext = new AutofacInterceptionContext(invocation);

           serviceInterceptor.Intercept(interceptionContext);
        }
    }
}