using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqEventRiser : ImAnHmqEventRiser, ImADependency
    {
        ImAnHmqActorAndReActorBookkeeper actorAndReActorBookkeeper;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            actorAndReActorBookkeeper = dependencyProvider.Get<ImAnHmqActorAndReActorBookkeeper>();
        }

        public async Task<OperationResult<ImAnHmqReActor>[]> Raise(HmqEvent hmqEvent)
        {
            ImAnHmqReActor[] allKnownReactors = actorAndReActorBookkeeper.GetAllReActors();

            if (allKnownReactors?.Any() != true)
                return Array.Empty<OperationResult<ImAnHmqReActor>>();

            OperationResult<ImAnHmqReActor>[] results = await Task.WhenAll(allKnownReactors.Select(r => Raise(hmqEvent, r)));

            return results;
        }

        async Task<OperationResult<ImAnHmqReActor>> Raise(HmqEvent hmqEvent, ImAnHmqReActor reactor)
        {
            OperationResult<ImAnHmqReActor> result = OperationResult.Fail("Not yet started").WithPayload(reactor);

            await
                new Func<Task>(async () =>
                {
                    result = (await reactor.Handle(hmqEvent)).WithPayload(reactor);
                })
                .TryOrFailWithGrace(
                    onFail: ex => result = OperationResult.Fail(ex, $"Error occurred while trying to handle {hmqEvent.Name}({hmqEvent.ID}) that happened on {hmqEvent.HappenedAt.PrintDateAndTime()}. Message: {ex.Message}").WithPayload(reactor)
                );

            return result;
        }
    }
}
