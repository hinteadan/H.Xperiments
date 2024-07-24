using H.Necessaire;
using H.Necessaire.Runtime.CLI;
using H.Necessaire.Runtime.CLI.Commands;
using Raven.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.RavenDb
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
