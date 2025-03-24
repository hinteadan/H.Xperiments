using H.Necessaire;
using H.Necessaire.CLI.Commands;

namespace H.Xperiments.RavenDb
{
    [Alias("raven")]
    internal class RavenDbCommand : CommandBase
    {
        protected override string[] GetUsageSyntaxes()
        {
            return [
                "raven|ravendb serve-embedded|start|serve [open-studio]",
                "raven|ravendb stop-embedded|stop",
                "",
            ];
        }

        public override async Task<OperationResult> Run()
        {
            Log("Running RavenDB Command...");
            using (new TimeMeasurement(x => Log($"DONE Running RavenDB Command in {x}")))
            {
                return await RunSubCommand();
            }
        }
    }
}
