using H.Necessaire;
using H.Necessaire.CLI.Commands;

namespace H.Xperiments.DotNetStuff
{
    [Alias("th")]
    internal class ThreadingCommand : CommandBase
    {
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
