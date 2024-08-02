using H.MQ.Abstractions;
using H.Necessaire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H.MQ.Concrete
{
    internal class HmqReActor : HmqActorIdentity, ImAnHmqReActor, ImADependency
    {
        internal Func<HmqEvent, Task<OperationResult>> Handler { get; set; }
        internal bool IsHandlingInternalEvents { get; set; } = true;
        internal bool IsHandlingExternalEvents { get; set; } = true;
        internal string[] SpecificHandledSourceIDs { get; set; }
        internal string[] SpecificHandledEventNames { get; set; }
        internal string[] SpecificHandledEventTypes { get; set; }

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

            if (!IsHandlingInternalEvents && !IsHandlingExternalEvents)
                return OperationResult.Win();

            if (hmqEvent.IsInternal() && !IsHandlingInternalEvents)
                return OperationResult.Win();

            if (hmqEvent.IsExternal() && !IsHandlingExternalEvents)
                return OperationResult.Win();

            if (SpecificHandledSourceIDs?.Any() == true && hmqEvent.RaisedByID.NotIn(SpecificHandledSourceIDs))
                return OperationResult.Win();

            if (SpecificHandledEventNames?.Any() == true && hmqEvent.Name.NotIn(SpecificHandledEventNames))
                return OperationResult.Win();

            if (SpecificHandledEventTypes?.Any() == true && hmqEvent.Type.NotIn(SpecificHandledEventTypes))
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
