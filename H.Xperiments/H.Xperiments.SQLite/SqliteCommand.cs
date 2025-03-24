using H.Necessaire;
using H.Necessaire.CLI.Commands;

namespace H.Xperiments.SQLite
{
    internal class SqliteCommand : CommandBase
    {
        protected override string[] GetUsageSyntaxes()
        {
            return [
                "sqlite debug",
                "",
            ];
        }

        public override async Task<OperationResult> Run()
        {
            Log("Running SQLite Command...");
            using (new TimeMeasurement(x => Log($"DONE Running SQLite Command in {x}")))
            {
                return await RunSubCommand();
            }
        }
    }
}
