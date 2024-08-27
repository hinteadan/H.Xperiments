using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    internal class PollyCommand : CommandBase
    {
        public override async Task<OperationResult> Run()
        {
            await Task.CompletedTask;

            Log($"Running Polly Command");
            using (new TimeMeasurement(x => Log($"DONE Running Polly Command in {x}")))
            {

            }

            return OperationResult.Win();
        }
    }
}
