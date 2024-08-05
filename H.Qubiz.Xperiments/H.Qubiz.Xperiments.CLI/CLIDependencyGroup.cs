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
                //.WithHmqFileSystemMessageBus()
                //.WithHmqFileSystemRuntime()
                //.StartHmqFileSystemExternalListener()
                //.WithHmqRavenDbRuntime()
                .WithHmqSqlServerRuntime()
                .StartHmqPeriodicPollingExternalListener()
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
        }
    }
}
