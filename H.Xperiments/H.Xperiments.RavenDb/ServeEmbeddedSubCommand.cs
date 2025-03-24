using H.Necessaire;
using H.Necessaire.CLI.Commands;
using Raven.Embedded;

namespace H.Xperiments.RavenDb
{
    [ID("serve-embedded")]
    [Alias("serve", "start")]
    internal class ServeEmbeddedSubCommand : SubCommandBase
    {
        public override async Task<OperationResult> Run(params Note[] args)
        {
            await Logger.LogInfo("Running RavenDB serve-embedded Command...");
            using (new TimeMeasurement(x => Logger.LogInfo($"DONE Running RavenDB serve-embedded Command in {x}").ConfigureAwait(false).GetAwaiter().GetResult()))
            {
                EmbeddedServer.Instance.StartServer();

                await Logger.LogInfo($"Running embedded RavenDB Server @ {await EmbeddedServer.Instance.GetServerUriAsync()}");

                if (args?.Any(a => a.ID.Is("open-studio")) == true)
                    EmbeddedServer.Instance.OpenStudioInBrowser();
            }

            return OperationResult.Win();
        }
    }
}
