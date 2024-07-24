using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    internal class DebugCommand : CommandBase
    {
        public override async Task<OperationResult> Run()
        {
            Log("Debugging...");
            using (new TimeMeasurement(x => Log($"DONE Debugging in {x}")))
            {
                await Task.Delay(0);
                Log("Debug Command");
            }

            return OperationResult.Win();
        }
    }
}
