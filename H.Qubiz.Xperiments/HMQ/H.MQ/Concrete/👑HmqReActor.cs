using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqReActor : HmqActorIdentity, ImAnHmqReActor, ImADependency
    {
        internal Func<HmqEvent, Task<OperationResult>> Handler { get; set; }
        ImAnHmqEventReActionRegistry eventReActingRegistry;
        ImALogger logger;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            eventReActingRegistry = dependencyProvider.Get<ImAnHmqEventReActionRegistry>();
            logger = dependencyProvider.GetLogger<HmqReActor>();
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

                    result = (await Handler(hmqEvent)) ?? OperationResult.Fail($"Handler for {ID} returned NULL as OperationResult");

                    OperationResult logResult = await eventReActingRegistry.LogEventReAction(hmqEvent, result.WithPayload(this.ToIdentityOnly()));
                    if (!logResult.IsSuccessful)
                    {
                        await logger.LogError($"Error occurred while trying to LogEventReAction for {hmqEvent} event in {ID} ReActor");
                    }

                })
                .TryOrFailWithGrace(onFail: ex => result = OperationResult.Fail(ex, $"Error ocurred in {ID} ReActor while handling {hmqEvent.Name} event. Message: {ex.Message}"));

            return result;
        }
    }
}
