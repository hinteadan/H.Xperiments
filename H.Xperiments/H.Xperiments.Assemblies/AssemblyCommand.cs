using H.Necessaire;
using H.Necessaire.CLI.Commands;
using H.Necessaire.Runtime.CLI.Commands;

namespace H.Xperiments.Assemblies
{
    [Alias("ass")]
    internal class AssemblyCommand : CommandBase
    {
        protected override string[] GetUsageSyntaxes()
        {
            return [
                "assembly|ass debug",
                "",
            ];
        }

        public override async Task<OperationResult> Run()
        {
            Log($"Running {GetType().Name}...");
            using (new TimeMeasurement(x => Log($"DONE Running {GetType().Name} in {x}")))
            {
                return await RunSubCommand();
            }
        }
    }
}
