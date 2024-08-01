using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqEventRegistry : ImAnHmqEventRegistry, ImADependency
    {
        ImAStorageService<Guid, HmqEvent> eventStore;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventStore = dependencyProvider.Get<ImAStorageService<Guid, HmqEvent>>();
        }

        public Task<OperationResult> Append(ImAnHmqEvent hmqEvent)
        {
            throw new System.NotImplementedException();
        }

        public Task<OperationResult<IDisposableEnumerable<ImAnHmqEvent>>> Stream(HmqEventFilter filter)
        {
            throw new System.NotImplementedException();
        }

        public Task<OperationResult<IDisposableEnumerable<ImAnHmqEvent>>> StreamAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
