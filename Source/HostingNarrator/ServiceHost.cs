﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using DynamicServiceHost.Host;
using DynamicServiceHost.Host.Abstracts;
using HostingNarrator.Abstracts;

namespace HostingNarrator
{
    public class ServiceHost : IDisposable
    {
        private readonly List<DynamicHost> dynamicHosts;

        public ServiceHost(List<DynamicHost> dynamicHosts)
        {
            this.dynamicHosts = dynamicHosts;
        }

        public static ServiceHost HostCommands(List<Type> serviceTypes,
            ModuleBuilder moduleBuilder,
            IHostContainer hostContainer,
            ILogger logger)
        {
            var hostList = new List<DynamicHost>();

            foreach (var serviceType in serviceTypes)
            {
                var host = new DynamicHost(serviceType, hostContainer/*, moduleBuilder: moduleBuilder*/);

                host.CreateConnectedHost(null);
                host.CreateDisconnectedHost();

                host.Open();

                logger.Log($"Connected and disconnected services hosted for: {serviceType.FullName}", LogLevel.Normal);

                hostList.Add(host);
            }

            return new ServiceHost(hostList);
        }

        public static ServiceHost HostQueries(List<Type> queryTypes,
            ModuleBuilder moduleBuilder,
            IHostContainer container,
            ILogger logger)
        {
            var hostList = new List<DynamicHost>();

            foreach (var serviceType in queryTypes)
            {
                var host = new DynamicHost(serviceType, container/*, moduleBuilder: moduleBuilder*/);

                host.CreateConnectedHost(isTransactional: false);

                host.Open();

                logger.Log($"Connected service hosted for: {serviceType.FullName}", LogLevel.Normal);

                hostList.Add(host);
            }

            return new ServiceHost(hostList);
        }

        public void Dispose()
        {
            foreach (var dynamicHost in dynamicHosts)
            {
                dynamicHost.Dispose();
            }

            dynamicHosts.Clear();
        }
    }
}