using System.Threading.Tasks;

namespace H.Necessaire.CLI.Commands
{
    internal interface ImACliSubCommand
    {
        Task<OperationResult> Run(params Note[] args);
    }
}
