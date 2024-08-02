using H.MQ.Abstractions;
using H.Necessaire;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqActor : HmqActorIdentity, ImAnHmqActor, ImADependency
    {
        private ImAnHmqActorIdentity identity = new HmqActorIdentity();
        ImAnHmqEventRegistry eventRegistry;
        ImAnHmqEventReActionRegistry eventReActingRegistry;
        ImAnHmqEventRiser eventRiser;

        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventRegistry = dependencyProvider.Get<ImAnHmqEventRegistry>();
            eventReActingRegistry = dependencyProvider.Get<ImAnHmqEventReActionRegistry>();
            eventRiser = dependencyProvider.Get<ImAnHmqEventRiser>();
        }

        public async Task<OperationResult> Raise(HmqEvent hmqEvent)
        {
            OperationResult appendResult = await eventRegistry.Append(hmqEvent);

            if (!appendResult.IsSuccessful)
                return appendResult;

            OperationResult<ImAnHmqReActor>[] raiseResults = await eventRiser.Raise(hmqEvent);

            OperationResult logResult = await eventReActingRegistry.LogEventReAction(hmqEvent, raiseResults.Select(x => x.WithPayload(x.Payload as ImAnHmqActorIdentity)).ToArray());

            if (!logResult.IsSuccessful)
                return logResult;

            OperationResult<OperationResult<ImAnHmqReActor>[]> globalRaiseResult = raiseResults.Merge(globalReasonIfNecesarry: "Some of the HMQ ReActors failed to handle the event. Check payload for details.").WithPayload(raiseResults.Where(x => !x.IsSuccessful).ToArrayNullIfEmpty());

            if (!globalRaiseResult.IsSuccessful)
                await HandleRaiseFailures(hmqEvent, globalRaiseResult.Payload.Select(x => x.Payload).ToArray());

            return globalRaiseResult;
        }

        private Task HandleRaiseFailures(HmqEvent hmqEvent, ImAnHmqReActor[] imAnHmqReActors)
        {
            //TODO: Handle raise failures if necessary
            return Task.CompletedTask;
        }
    }
}
