using H.MQ.Abstractions;
using H.Necessaire;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqActor : HmqActorIdentity, ImAnHmqActor, ImADependency
    {
        ImAnHmqEventRegistry eventRegistry;
        ImAnHmqEventRiser eventRiser;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventRegistry = dependencyProvider.Get<ImAnHmqEventRegistry>();
            eventRiser = dependencyProvider.Get<ImAnHmqEventRiser>();
        }

        public async Task<OperationResult> Raise(HmqEvent hmqEvent)
        {
            hmqEvent = hmqEvent.Clone().And(x => x.RaisedBy = this.ToIdentityOnly());

            OperationResult appendResult = await eventRegistry.Append(hmqEvent);

            if (!appendResult.IsSuccessful)
                return appendResult;

            OperationResult<ImAnHmqReActor>[] raiseResults = await eventRiser.Raise(hmqEvent);

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
