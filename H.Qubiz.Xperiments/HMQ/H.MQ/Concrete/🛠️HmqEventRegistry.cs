using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqEventRegistry : ImAnHmqEventRegistry, ImAnHmqEventReActionRegistry, ImADependency
    {
        ImAStorageService<Guid, HmqEvent> eventStore;
        ImAStorageBrowserService<HmqEvent, HmqEventFilter> eventBrowser;
        ImAStorageService<Guid, HmqEventReactionLog> eventReactionLogStore;
        ImAStorageBrowserService<HmqEventReactionLog, HmqEventReActionFilter> eventReactionLogBrowser;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventStore = dependencyProvider.Get<ImAStorageService<Guid, HmqEvent>>();
            eventBrowser = dependencyProvider.Get<ImAStorageBrowserService<HmqEvent, HmqEventFilter>>();
            eventReactionLogStore = dependencyProvider.Get<ImAStorageService<Guid, HmqEventReactionLog>>();
            eventReactionLogBrowser = dependencyProvider.Get<ImAStorageBrowserService<HmqEventReactionLog, HmqEventReActionFilter>>();
        }

        public async Task<OperationResult> Append(HmqEvent hmqEvent)
        {
            if (hmqEvent is null)
                return OperationResult.Fail("Event is NULL");

            HmqEvent eventToSave = hmqEvent.Clone();
            eventToSave.Attributes = hmqEvent.Attributes.AddOrReplace(
                $"{true}".NoteAs("IsPersisted"),
                "PersistentStore".NoteAs("Source")
            );

            return await eventStore.Save(eventToSave);
        }

        public async Task<OperationResult<IDisposableEnumerable<HmqEvent>>> Stream(HmqEventFilter filter)
        {
            OperationResult<IDisposableEnumerable<HmqEvent>> streamResult = await eventBrowser.Stream(filter);
            return streamResult?.WithPayload(streamResult?.Payload?.ProjectTo(x => x as HmqEvent));
        }

        public async Task<OperationResult<IDisposableEnumerable<HmqEvent>>> StreamAll()
        {
            OperationResult<IDisposableEnumerable<HmqEvent>> streamResult = await eventBrowser.StreamAll();
            return streamResult?.WithPayload(streamResult?.Payload?.ProjectTo(x => x as HmqEvent));

        }

        public async Task<OperationResult> LogEventReAction(HmqEvent hmqEvent, params OperationResult<ImAnHmqActorIdentity>[] hmqReActorResults)
        {
            if (hmqReActorResults?.Any() != true)
                return OperationResult.Win();


            OperationResult[] logResults = new OperationResult[hmqReActorResults.Length];
            int index = -1;
            foreach (OperationResult<ImAnHmqActorIdentity> hmqReActorResult in hmqReActorResults)
            {
                index++;
                logResults[index] = await LogEventReAction(hmqEvent, hmqReActorResult);
            }

            OperationResult result = logResults.Merge(globalReasonIfNecesarry: $"Some event reactions couldn't be logged. See reasons for details.");

            return result;
        }

        private async Task<OperationResult> LogEventReAction(HmqEvent hmqEvent, OperationResult<ImAnHmqActorIdentity> hmqReActorResult)
        {
            OperationResult result = OperationResult.Fail("Not yet started");

            await
                new Func<Task>(async () =>
                {
                    HmqEventReactionLog reactionLog = BuildReactionLog(hmqEvent, hmqReActorResult);

                    if (reactionLog is null)
                    {
                        result = OperationResult.Win();
                        return;
                    }

                    result = await eventReactionLogStore.Save(reactionLog);

                })
                .TryOrFailWithGrace(
                    onFail: ex => result = OperationResult.Fail(ex, $"Error ocurred while trying to Log {hmqEvent.Name} Event ReAction of {hmqReActorResult.Payload.ID}")
                );

            return result;
        }

        private HmqEventReactionLog BuildReactionLog(HmqEvent hmqEvent, OperationResult<ImAnHmqActorIdentity> hmqReActorResult)
        {
            if (hmqEvent is null)
                return null;

            if (hmqReActorResult is null)
                return null;

            return
                new HmqEventReactionLog
                {
                    ID = Guid.NewGuid(),
                    ActorIdentity = hmqReActorResult.Payload.ToIdentityOnly(),
                    AsOf = DateTime.UtcNow,
                    Event = hmqEvent,
                    OperationResult = new OperationResult { 
                        Comments = hmqReActorResult.Comments,
                        IsSuccessful = hmqReActorResult.IsSuccessful,
                        Reason = hmqReActorResult.Reason,
                    },
                };
        }
    }
}
