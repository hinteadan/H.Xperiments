using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqReActor : HmqActorIdentity, ImAnHmqReActor, ImADependency
    {
        internal Func<HmqEvent, Task<OperationResult>> Handler { get; set; }
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            
        }

        public async Task<OperationResult> Handle(HmqEvent hmqEvent)
        {
            if (Handler is null)
                return OperationResult.Win();

            if (hmqEvent is null)
                return OperationResult.Win();

            OperationResult result = OperationResult.Fail("Not yet started");

            await
                new Func<Task>(async () => { 

                    result = await Handler(hmqEvent);

                })
                .TryOrFailWithGrace(onFail: ex => result = OperationResult.Fail(ex, $"Error ocurred in {ID} ReActor while handling {hmqEvent.Name} event. Message: {ex.Message}"));

            return result;
        }
    }
}
