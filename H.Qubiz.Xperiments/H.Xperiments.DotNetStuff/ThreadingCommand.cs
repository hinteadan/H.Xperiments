using H.Necessaire;
using H.Necessaire.CLI.Commands;
using System.Threading.Tasks;
using System.Timers;

namespace H.Xperiments.DotNetStuff
{
    [Alias("th")]
    internal class ThreadingCommand : CommandBase
    {
        private readonly Timer timer = new Timer { AutoReset = false };

        public override async Task<OperationResult> Run()
        {
            Log("Running DotNet Threading Command...");
            using (new TimeMeasurement(x => Log($"DONE Running DotNet Threading Command in {x}")))
            {
                await Task.Delay(0);


            }

            return OperationResult.Win();
        }
    }
}
