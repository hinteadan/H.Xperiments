using H.MQ.Abstractions;
using H.MQ.Core;
using H.MQ.RavenDB.Concrete;
using H.MQ.RavenDB.Concrete.Storage;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.MQ.FileSystem.Concrete
{
    internal class RavenDbHmqEventRiser : ImAnHmqEventRiser, ImADependency
    {
        RavenDbServiceBusStorageService ravenDbServiceBusStore;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            ravenDbServiceBusStore = dependencyProvider.Get<RavenDbServiceBusStorageService>();
        }

        public async Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent)
        {
            HmqEvent eventToSend = hmqEvent.Clone().MarkAsPersisted();

            await ravenDbServiceBusStore.Save(eventToSend);

            return OperationResult.Win().WithPayload(RavenDbReActor.Instance).AsArray();
        }
    }
}
