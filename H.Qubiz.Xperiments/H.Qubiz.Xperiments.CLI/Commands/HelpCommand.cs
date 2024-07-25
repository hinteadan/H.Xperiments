using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using H.Qubiz.Xperiments.CLI.BLL;
using System.Linq;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [Alias("?")]
    internal class HelpCommand : CommandBase
    {
        static readonly string[] usageSyntax = [
            "help|?",
            "help|? [searchKey:string]",
            "help|? [q=searchKey:string]",
        ];
        protected override string[] GetUsageSyntaxes() => usageSyntax;

        public override async Task<OperationResult> Run()
        {
            Note[] args = (await GetArguments()).Jump(1);

            string searchKey
                = args.Length == 1
                ? (args.Single().Value.IsEmpty() ? args.Single().ID : args.Single().Value)
                : args.Get("q", ignoreCase: true)
                ;

            CliCommandHelpInfo[] commandsToShowHelpFor = CliCommandsIndexer.FindCliCommands(searchKey);

            commandsToShowHelpFor.PrintToConsole();

            return OperationResult.Win();
        }
    }
}
