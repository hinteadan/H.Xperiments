using H.MQ.Abstractions;
using H.MQ;
using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    internal class HmqCommand : CommandBase
    {
        ImAnHmqActor debugActor;
        ImAnHmqReActor debugReActor;
        public override void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            base.ReferDependencies(dependencyProvider);
            debugActor = dependencyProvider.GetHmqActor("debug");
            debugReActor = dependencyProvider.GetHmqReActor(InProcessReact, "debug");
        }

        public override async Task<OperationResult> Run()
        {
            Log("HMQing...");
            using (new TimeMeasurement(x => Log($"DONE HMQing in {x}")))
            {
                await Task.CompletedTask;

                OperationResult raiseResult = await debugActor.Raise("test".ToHmqEvent());
            }

            return OperationResult.Win();
        }

        async Task<OperationResult> InProcessReact(HmqEvent hmqEvent)
        {
            await Task.CompletedTask;

            return OperationResult.Win();
        }
    }
}
