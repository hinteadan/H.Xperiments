using H.Necessaire;
using H.Necessaire.CLI.Commands;

namespace H.Xperiments.Assemblies
{
    internal class DebugSubCommand : SubCommandBase
    {
        public override async Task<OperationResult> Run(params Note[] args)
        {
            Log($"Running {GetType().Name}...");
            using (new TimeMeasurement(x => Log($"DONE Running {GetType().Name} in {x}")))
            {

            }

            return OperationResult.Win();
        }
    }
}
