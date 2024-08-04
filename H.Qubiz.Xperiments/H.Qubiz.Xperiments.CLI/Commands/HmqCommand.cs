using H.MQ.Abstractions;
using H.MQ;
using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System.Threading.Tasks;
using H.MQ.Core;
using System;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    internal class HmqCommand : CommandBase
    {
        ImAnHmqActor debugActor;
        ImAnHmqReActor debugReActor;
        ImAnHmqReActor extDebugReActor;
        public override void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            base.ReferDependencies(dependencyProvider);
            debugActor = dependencyProvider.GetHmqActor("debug");
            debugReActor = dependencyProvider.GetCatchAllInternalHmqReActor(InProcessReact, "debug");
            extDebugReActor = dependencyProvider.GetCatchAllExternalHmqReActor(ExternalReact, "extDebug");
        }

        public override async Task<OperationResult> Run()
        {
            Log("HMQing...");
            using (new TimeMeasurement(x => Log($"DONE HMQing in {x}")))
            {
                await Task.CompletedTask;

                OperationResult raiseResult = await debugActor.Raise(new SomeFancyPayload().ToHmqEvent());
            }

            return OperationResult.Win();
        }

        async Task<OperationResult> InProcessReact(HmqEvent hmqEvent)
        {
            await Task.CompletedTask;

            return OperationResult.Win();
        }

        async Task<OperationResult> ExternalReact(HmqEvent hmqEvent)
        {
            await Task.CompletedTask;

            return OperationResult.Win();
        }

        class SomeFancyPayload
        {
            public Guid ID { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = Guid.NewGuid().ToString();
            public DateTime DateTime { get; set; } = DateTime.UtcNow;
        }
    }
}
