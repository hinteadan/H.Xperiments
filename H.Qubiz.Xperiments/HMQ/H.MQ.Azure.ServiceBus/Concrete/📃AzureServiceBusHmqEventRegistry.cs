using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Azure.ServiceBus.Concrete
{
    internal class AzureServiceBusHmqEventRegistry : ImAnHmqEventRegistry, ImAnHmqEventReActionRegistry, ImADependency
    {
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Append(HmqEvent hmqEvent)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> LogEventReAction(HmqEvent hmqEvent, params OperationResult<ImAnHmqActorIdentity>[] hmqReActorResults)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IDisposableEnumerable<HmqEvent>>> Stream(HmqEventFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IDisposableEnumerable<HmqEventReactionLog>>> Stream(HmqEventReActionFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<IDisposableEnumerable<HmqEvent>>> StreamAll()
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<IDisposableEnumerable<HmqEventReactionLog>>> ImAnHmqEventReActionRegistry.StreamAll()
        {
            throw new NotImplementedException();
        }
    }
}
