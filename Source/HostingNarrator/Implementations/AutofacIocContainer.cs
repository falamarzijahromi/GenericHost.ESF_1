using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.ResolveAnything;
using Composition.ESF_1;
using HostingNarrator.Abstracts;

namespace HostingNarrator.Implementations
{
    public class AutofacIocContainer : IIocContainer, IDisposable
    {
        private ContainerBuilder containerBuilder;
        private readonly ILogger logger;

        public AutofacIocContainer(ILogger logger)
        {
            containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            this.logger = logger;

            RegisterGenericInterceptor();
        }

        public void RegisterFactory(
            Type[] serviceTypes, 
            Func<object> serviceFactory, 
            Type[] interceptors = null)
        {
            var builder = containerBuilder
                .Register(c => serviceFactory())
                .InstancePerLifetimeScope();

            foreach (var serviceType in serviceTypes)
            {
                builder.As(serviceType);
            }

            RegisterInterceptors(interceptors, builder);

            ProvideLoggingContents(serviceTypes, interceptors, out StringBuilder serviceNames, out StringBuilder interceptorNames);

            logger.Log($"Service factory: {serviceFactory} registered as:{Environment.NewLine}{serviceNames} with interceptors as following: {interceptorNames}", LogLevel.All);
        }

        public void RegisterTransient(Type from, Type to)
        {
            containerBuilder
                .RegisterType(to)
                .As(from)
                .InstancePerDependency();

            logger.Log($"Service {to.FullName} registered as {from.FullName} with transient life style", LogLevel.All);
        }

        public void RegisterSingleton<T>(T instance)
        {
            containerBuilder
                .Register(c => instance)
                .As<T>()
                .SingleInstance();

            logger.Log($"Singleton {instance.GetType().FullName} registered as {typeof(T).FullName}", LogLevel.All);
        }

        public void RegisterPerGraph(
            Type[] serviceTypes, 
            Type implementationType, 
            Type[] interceptorTypes = null)
        {
            var builder = containerBuilder
                .RegisterType(implementationType)
                .InstancePerLifetimeScope();

            foreach (var serviceType in serviceTypes)
            {
                builder.As(serviceType);
            }

            var setInterceptors = SetInterceptorTypes(interceptorTypes);

            RegisterInterceptors(setInterceptors, builder);

            ProvideLoggingContents(serviceTypes, interceptorTypes, out StringBuilder serviceNames, out StringBuilder interceptorNames);

            logger.Log(
                $"Service: {implementationType} registered as:{Environment.NewLine}{serviceNames}with interceptors as following:{Environment.NewLine}{interceptorNames}",
                LogLevel.All);
        }

        public IContainer CreateContainer()
        {
            return containerBuilder.Build();
        }

        public void Dispose()
        {
            containerBuilder = null;
        }

        private Type[] SetInterceptorTypes(Type[] interceptorTypes)
        {
            interceptorTypes = interceptorTypes ?? new Type[0];

            var interceptors = new List<Type>();

            interceptors.Add(typeof(LogInterceptor));

            interceptors.AddRange(interceptorTypes);

            return interceptors.ToArray();
        }

        private void RegisterInterceptors(
            Type[] interceptorTypes, 
            IRegistrationBuilder<object, IConcreteActivatorData, SingleRegistrationStyle> builder)
        {
            if (interceptorTypes != null && interceptorTypes.Any())
            {
                if (!interceptorTypes.All(type => typeof(IServiceInterceptor).IsAssignableFrom(type)))
                {
                    var notSuitableTypes = interceptorTypes
                        .Where(type => !typeof(IServiceInterceptor).IsAssignableFrom(type))
                        .Aggregate(new StringBuilder(), (seed, type) => seed.Append(type + ", "));

                    throw new Exception($"Types: {notSuitableTypes} must be IServiceInterceptor to be registered as an interceptor");
                }

                builder.EnableInterfaceInterceptors();

                foreach (var interceptorType in interceptorTypes)
                {
                    containerBuilder.RegisterType(interceptorType)
                        .InstancePerLifetimeScope()
                        .AsSelf();

                    var interceptor = CreateInterceptor(interceptorType);

                    builder.InterceptedBy(interceptor);
                }
            }
        }

        private Type CreateInterceptor(Type interceptorType)
        {
            var genInterceptorType = typeof(IGenericInterceptor<>);

            var interceptor = genInterceptorType.MakeGenericType(new Type[]{interceptorType});

            return interceptor;
        }

        private void RegisterGenericInterceptor()
        {
            containerBuilder
                .RegisterGeneric(typeof(GenericInterceptor<>))
                .As(typeof(IGenericInterceptor<>))
                .InstancePerLifetimeScope();

            logger.Log($"Ioc container registered generic interceptor {typeof(GenericInterceptor<>).FullName} as {typeof(IGenericInterceptor<>).FullName}", LogLevel.All);
        }

        private void ProvideLoggingContents(Type[] serviceTypes, Type[] interceptors, out StringBuilder serviceNames, out StringBuilder interceptorNames)
        {
            serviceNames = serviceTypes.Aggregate(new StringBuilder(), (seed, type) => seed.Append(type.FullName + Environment.NewLine));

            interceptorNames = new StringBuilder();

            if (interceptors != null && interceptors.Any())
            {
                interceptorNames = interceptors.Aggregate(new StringBuilder(), (seed, type) => seed.Append(type.FullName + Environment.NewLine));
            }
        }
    }
}