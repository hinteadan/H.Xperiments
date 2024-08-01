using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqEventRiser : ImAnHmqEventRiser, ImADependency
    {
        ImAnHmqReActor[] allKnownReactors;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            allKnownReactors
                = allKnownReactors 
                ??
                typeof(ImAnHmqReActor)
                .GetAllImplementations()
                .Select(t => (dependencyProvider.Get(t) ?? Activator.CreateInstance(t)) as ImAnHmqReActor)
                .ToNoNullsArray()
                ;
        }

        public async Task<OperationResult> Raise(ImAnHmqEvent hmqEvent)
        {
            if (allKnownReactors?.Any() != true)
                return OperationResult.Win();

            OperationResult[] results = await Task.WhenAll(allKnownReactors.Select(r => Raise(hmqEvent, r)));

            return results.Merge();
        }

        async Task<OperationResult> Raise(ImAnHmqEvent hmqEvent, ImAnHmqReActor reactor)
        {
            OperationResult result = OperationResult.Fail("Not yet started");

            await
                new Func<Task>(async () =>
                {
                    result = await reactor.Handle(hmqEvent);
                })
                .TryOrFailWithGrace(
                    onFail: ex => result = OperationResult.Fail(ex, $"Error occurred while trying to handle {hmqEvent.Name}({hmqEvent.ID}) that happened on {hmqEvent.HappenedAt.PrintDateAndTime()}. Message: {ex.Message}")
                );

            return result;
        }
    }
}
