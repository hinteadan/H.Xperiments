using H.Necessaire.CLI.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H.Necessaire.Runtime.CLI.Commands
{
    public abstract class ExtendedCommandBase : CommandBase
    {
        Func<string, ImACliSubCommand> subCommandFinder;
        public override void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            base.ReferDependencies(dependencyProvider);
            subCommandFinder = subCommandFinder ?? (name => dependencyProvider.Build<ImACliSubCommand>(name));
        }

        protected virtual async Task<OperationResult> RunSubCommand(bool failOnMissingCommand = false)
        {
            Note[] args = (await GetArguments())?.Jump(1);

            if (args?.Any() != true)
            {
                return failOnMissingCommand ? OperationResult.Fail("No Args") : WinWithUsageSyntax();
            }

            ImACliSubCommand subCommand = subCommandFinder.Invoke(args.First().ID);

            if (subCommand is null)
            {
                return failOnMissingCommand ? OperationResult.Fail("No Matching Sub-Command") : WinWithUsageSyntax();
            }

            return await subCommand.Run(args.Jump(1));
        }

        protected virtual OperationResult WinWithUsageSyntax(string reason = null)
        {
            Log($"{reason}{Environment.NewLine}{Environment.NewLine}{PrintUsageSyntax()}");
            return OperationResult.Win();
        }
    }
}
