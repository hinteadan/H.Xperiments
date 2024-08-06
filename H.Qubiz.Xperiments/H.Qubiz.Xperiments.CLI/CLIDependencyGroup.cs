using H.MQ;
using H.MQ.Azure.ServiceBus;
using H.MQ.Core;
using H.MQ.FileSystem;
using H.MQ.RabbitMQ;
using H.MQ.RavenDB;
using H.MQ.Runtime.FileSystem;
using H.MQ.Runtime.RavenDb;
using H.MQ.Runtime.SqlServer;
using H.Necessaire;
using H.Necessaire.WebSockets;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;

namespace H.Qubiz.Xperiments.CLI
{
    internal class CLIDependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .WithHmq()

                //.WithAzureServiceBusHmq()
                //.StartHmqAzureServiceBusExternalListener()
                .WithHmqFileSystemMessageBus()
                .WithHmqFileSystemRuntime()
                .StartHmqFileSystemExternalListener()
                //.WithHmqRavenDbRuntime()
                //.WithHmqSqlServerRuntime()
                //.StartHmqPeriodicPollingExternalListener()
                //.WithHmqRavenDbMessageBus()
                //.StartHmqRavenDbExternalListener()
                //.WithHmqRabbitMqMessageBus()
                //.StartHmqRabbitMqExternalListener()

                .Register<RavenDb.DependencyGroup>(() => new RavenDb.DependencyGroup())
                .Register<SQLite.DependencyGroup>(() => new SQLite.DependencyGroup())
                .Register<BLL.CliCommandsIndexer>(() => new BLL.CliCommandsIndexer())
                .Register<H.Xperiments.Assemblies.DependencyGroup>(() => new H.Xperiments.Assemblies.DependencyGroup())
                .Register<H.Xperiments.CLI.UI.DependencyGroup>(() => new H.Xperiments.CLI.UI.DependencyGroup())

                ;

            IServiceProvider serviceProvider = new ServiceProvider(dependencyRegistry);

            Necessaire.WebSockets.DependencyInjectionExtensions.AddHNecessaireWebSockets(
                singletonRegistrarByType: t => dependencyRegistry.Register(t, () => Activator.CreateInstance(t)),
                singletonRegistrarByInterfaceType: (i, t) => dependencyRegistry.Register(i, () => Activator.CreateInstance(t)),
                singletonRegistrarByFactory: (t, f) => dependencyRegistry.Register(t, () => f.Invoke(serviceProvider)),
                webSocketServerBaseUrlProvider: x => "ws://localhost:11080"
                );
        }

        class ServiceProvider : IServiceProvider
        {
            readonly ImADependencyProvider dependencyProvider;
            public ServiceProvider(ImADependencyProvider dependencyProvider)
            {
                this.dependencyProvider = dependencyProvider;
            }

            public object GetService(Type serviceType) => dependencyProvider.Get(serviceType);
        }
    }
}
