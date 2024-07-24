using H.Necessaire;
using H.Necessaire.Runtime.CLI;
using H.Necessaire.Runtime.CLI.Commands;
using Raven.Embedded;

namespace H.Qubiz.Xperiments.RavenDb
{
    [ID("serve-embedded")]
    [Alias("serve", "start")]
    internal class ServeEmbeddedSubCommand : SubCommandBase
    {
        public override async Task<OperationResult> Run(params Note[] args)
        {
            CLIPrinter.PrintLog("Running RavenDB serve-embedded Command...");
            using (new TimeMeasurement(x => CLIPrinter.PrintLog($"DONE Running RavenDB serve-embedded Command in {x}")))
            {
                EmbeddedServer.Instance.StartServer();

                CLIPrinter.PrintLog($"Running embedded RavenDB Server @ {await EmbeddedServer.Instance.GetServerUriAsync()}");

                if (args?.Any(a => a.ID.Is("open-studio")) == true)
                    EmbeddedServer.Instance.OpenStudioInBrowser();
            }

            return OperationResult.Win();
        }
    }
}
