using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqEventRegistry : ImAnHmqEventRegistry, ImADependency
    {
        ImAStorageService<Guid, HmqEvent> eventStore;
        ImAStorageBrowserService<HmqEvent, HmqEventFilter> eventBrowser;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventStore = dependencyProvider.Get<ImAStorageService<Guid, HmqEvent>>();
            eventBrowser = dependencyProvider.Get<ImAStorageBrowserService<HmqEvent, HmqEventFilter>>();
        }

        public async Task<OperationResult> Append(ImAnHmqEvent hmqEvent)
        {
            if (hmqEvent is null)
                return OperationResult.Fail("Event is NULL");

            HmqEvent eventToSave = hmqEvent.Map();
            eventToSave.Attributes = hmqEvent.Attributes.AddOrReplace(
                $"{true}".NoteAs("IsPersisted"),
                "PersistentStore".NoteAs("Source")
            );

            return await eventStore.Save(eventToSave);
        }

        public async Task<OperationResult<IDisposableEnumerable<ImAnHmqEvent>>> Stream(HmqEventFilter filter)
        {
            OperationResult<IDisposableEnumerable<HmqEvent>> streamResult = await eventBrowser.Stream(filter);
            return streamResult?.WithPayload(streamResult?.Payload?.ProjectTo(x => x as ImAnHmqEvent));
        }

        public async Task<OperationResult<IDisposableEnumerable<ImAnHmqEvent>>> StreamAll()
        {
            OperationResult<IDisposableEnumerable<HmqEvent>> streamResult = await eventBrowser.StreamAll();
            return streamResult?.WithPayload(streamResult?.Payload?.ProjectTo(x => x as ImAnHmqEvent));

        }
    }
}
