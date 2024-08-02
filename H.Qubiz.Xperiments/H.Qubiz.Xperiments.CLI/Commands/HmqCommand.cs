using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    internal class HmqCommand : CommandBase
    {
        public override async Task<OperationResult> Run()
        {
            Log("HMQing...");
            using (new TimeMeasurement(x => Log($"DONE HMQing in {x}")))
            {
                await Task.CompletedTask;
                

            }

            return OperationResult.Win();
        }
    }
}
