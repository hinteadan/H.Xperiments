using H.Necessaire;
using H.Necessaire.CLI.Commands;
using Raven.Embedded;

namespace H.Xperiments.RavenDb
{
    [ID("stop-embedded")]
    [Alias("stop")]
    internal class StopEmbeddedSubCommand : SubCommandBase
    {
        public override async Task<OperationResult> Run(params Note[] args)
        {
            Log("Running RavenDB stop-embedded Command...");
            using (new TimeMeasurement(x => Log($"DONE Running RavenDB stop-embedded Command in {x}")))
            {
                await Task.CompletedTask;
                EmbeddedServer.Instance.Dispose();
            }

            return OperationResult.Win();
        }
    }
}
