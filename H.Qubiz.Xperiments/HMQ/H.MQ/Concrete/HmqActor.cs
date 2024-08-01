using H.MQ.Abstractions;
using H.Necessaire;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqActor : ImAnHmqActor, ImADependency
    {
        ImAnHmqEventRegistry eventRegistry;
        ImAnHmqEventRiser eventRiser;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventRegistry = dependencyProvider.Get<ImAnHmqEventRegistry>();
            eventRiser = dependencyProvider.Get<ImAnHmqEventRiser>();
        }

        public async Task<OperationResult> Raise(ImAnHmqEvent hmqEvent)
        {
            OperationResult appendResult = await eventRegistry.Append(hmqEvent);

            if (!appendResult.IsSuccessful)
                return appendResult;

            OperationResult<ImAnHmqReActor>[] raiseResults = await eventRiser.Raise(hmqEvent);

            OperationResult globalRaiseResult = raiseResults.Merge(globalReasonIfNecesarry: "Some of the HMQ ReActors failed to handle the event. Check payload for details.").WithPayload(raiseResults.Where(x => !x.IsSuccessful).ToArrayNullIfEmpty());

            return globalRaiseResult;
        }
    }
}
