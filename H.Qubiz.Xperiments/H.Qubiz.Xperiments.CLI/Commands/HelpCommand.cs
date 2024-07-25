using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using H.Qubiz.Xperiments.CLI.BLL;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("?")]
    internal class HelpCommand : CommandBase
    {
        public override Task<OperationResult> Run()
        {
            CliCommandHelpInfo[] commandsToShowHelpFor = CliCommandsIndexer.AllKnownCliCommands;

            commandsToShowHelpFor.PrintToConsole();

            return OperationResult.Win().AsTask();
        }
    }
}
