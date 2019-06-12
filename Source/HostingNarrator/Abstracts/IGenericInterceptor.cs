using Castle.DynamicProxy;
using Composition.ESF_1;

namespace HostingNarrator.Abstracts
{
    public interface IGenericInterceptor<T> : IInterceptor where T: IServiceInterceptor
    {
    }
}