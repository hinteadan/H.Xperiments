using H.MQ.Abstractions;
using H.Necessaire;
using System;

namespace H.MQ.Runtime.SqlServer.Concrete.Storage
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry

                .Register<SqlServerHmqEventStorageService>(() => new SqlServerHmqEventStorageService())
                .Register<ImAStorageService<Guid, HmqEvent>>(() => dependencyRegistry.Get<SqlServerHmqEventStorageService>())
                .Register<ImAStorageBrowserService<HmqEvent, HmqEventFilter>>(() => dependencyRegistry.Get<SqlServerHmqEventStorageService>())

                .Register<SqlServerHmqEventReActionStorageService>(() => new SqlServerHmqEventReActionStorageService())
                .Register<ImAStorageService<Guid, HmqEventReactionLog>>(() => dependencyRegistry.Get<SqlServerHmqEventReActionStorageService>())
                .Register<ImAStorageBrowserService<HmqEventReactionLog, HmqEventReActionFilter>>(() => dependencyRegistry.Get<SqlServerHmqEventReActionStorageService>())

                ;
        }
    }
}
