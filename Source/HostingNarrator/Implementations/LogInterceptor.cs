using Composition.ESF_1;
using HostingNarrator.Abstracts;
using System;

namespace HostingNarrator.Implementations
{
    public class LogInterceptor : IServiceInterceptor
    {
        private readonly ILogger logger;

        public LogInterceptor(ILogger logger)
        {
            this.logger = logger;
        }

        public void Intercept(IInterceptionContext interceptionContext)
        {
            try
            {
                interceptionContext.Proceed();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                throw;
            }

            logger.Log($"Method {interceptionContext.MethodName} of type {interceptionContext.TargetType.FullName} invoked", LogLevel.All);
        }
    }
}