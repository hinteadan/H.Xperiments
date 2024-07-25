using H.Necessaire;
using H.Necessaire.CLI.Commands;
using H.Necessaire.Runtime.CLI;
using H.Necessaire.Runtime.CLI.Commands;
using H.Qubiz.Xperiments.CLI.BLL;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [ID("cli")]
    internal class CommandInterpreterCommand : CommandBase
    {
        const string cliMarker = "(> ";
        static readonly string[] exitCommands = ["exit", "quit", "bye"];
        static readonly string[] helpCommands = ["help", "?"];
        readonly CancellationTokenSource commandCancelTokenSource = new CancellationTokenSource();

        public override async Task<OperationResult> Run()
        {
            Log("Starting Command Interpreter...");
            using (new TimeMeasurement(x => Log($"DONE Starting Command Interpreter in {x}")))
            {
                await Task.Delay(0);

                Console.CancelKeyPress += CancelKeyPress;

                WaitForUserInput(commandCancelTokenSource.Token);

                Log("NOTE: You can always force close via Ctr+C");
            }

            await Console.Out.WriteAsync(cliMarker);

            await KeepAlive();

            return OperationResult.Win();
        }

        private async Task OnUserInput(string userInput)
        {
            if (commandCancelTokenSource.IsCancellationRequested)
                return;

            OperationResult commandResult = await InterpretUserInput(userInput);

            if (commandResult?.IsSuccessful == false)
            {
                await Logger.LogError(string.Join(Environment.NewLine, commandResult.FlattenReasons()));
            }
        }

        private async Task KeepAlive()
        {
            while (!commandCancelTokenSource.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(-1, commandCancelTokenSource.Token);
                }
                catch (TaskCanceledException)
                {

                }
            }
        }

        private void WaitForUserInput(CancellationToken cancellationToken)
        {
            Task.Run(
                async () => {
                    (int Left, int Top) prevCursorPosition = Console.GetCursorPosition();

                    string userInput = await Console.In.ReadLineAsync(cancellationToken);
                    await OnUserInput(userInput);

                    (int Left, int Top) newCursorPosition = Console.GetCursorPosition();

                    if (!IsExitCommand(userInput) && prevCursorPosition != newCursorPosition)
                        await Console.Out.WriteAsync(cliMarker);

                    WaitForUserInput(cancellationToken);
                }, 
                cancellationToken
            );
        }

        private async Task<OperationResult> InterpretUserInput(string userInput)
        {
            if (IsExitCommand(userInput))
            {
                commandCancelTokenSource.Cancel();
                return OperationResult.Win();
            }

            if (IsHelpCommand(userInput))
            {
                return await RunCliHelpCommand();
            }

            if (userInput.IsEmpty())
            {
                return OperationResult.Win();
            }

            return await RunCliCommand(userInput?.Split(" ", StringSplitOptions.RemoveEmptyEntries) ?? []);
        }

        private async Task<OperationResult> RunCliHelpCommand()
        {
            CliCommandHelpInfo[] commandsToShowHelpFor = CliCommandsIndexer.AllKnownCliCommands;

            foreach (CliCommandHelpInfo commandHelpInfo in commandsToShowHelpFor)
            {
                PrintCommandHelpInfo(commandHelpInfo);
                Console.WriteLine("------");
                Console.WriteLine();
            }

            return OperationResult.Win();
        }

        private void PrintCommandHelpInfo(CliCommandHelpInfo commandHelpInfo)
        {
            using (new ScopedRunner(() => Console.ForegroundColor = ConsoleColor.Green, Console.ResetColor))
            {
                Console.WriteLine($"{commandHelpInfo.Name}");
                Console.WriteLine();
            }

            string preferredSyntax = commandHelpInfo.GetPreferredCommandSyntax();
            string[] otherSyntaxes = commandHelpInfo.GetAllCommandSyntaxes()?.Where(x => !x.Is(preferredSyntax)).ToArrayNullIfEmpty();

            if (!preferredSyntax.Is(commandHelpInfo.Name))
            {
                using (new ScopedRunner(() => Console.ForegroundColor = ConsoleColor.Yellow, Console.ResetColor))
                {
                    Console.WriteLine(preferredSyntax);
                }
            }

            if (otherSyntaxes?.Any() == true)
            {
                using (new ScopedRunner(() => Console.ForegroundColor = ConsoleColor.Gray, Console.ResetColor))
                {
                    Console.WriteLine(string.Join(" | ", otherSyntaxes));
                }
            }

            string[] usageSyntaxes = commandHelpInfo.UsageSyntaxes?.Where(x => !x.IsEmpty()).ToArrayNullIfEmpty();
            if (usageSyntaxes?.Any() == true)
            {
                Console.WriteLine();

                using (new ScopedRunner(() => Console.ForegroundColor = ConsoleColor.Cyan, Console.ResetColor))
                {
                    Console.WriteLine("Usage Syntax:");
                    Console.WriteLine("=============");
                }

                using (new ScopedRunner(() => Console.ForegroundColor = ConsoleColor.Gray, Console.ResetColor))
                {
                    foreach (string usageSyntax in usageSyntaxes)
                    {
                        Console.WriteLine(usageSyntax);
                    }
                }

                Console.WriteLine();
            }
        }

        private static bool IsExitCommand(string userInput) => IsCommand(userInput, exitCommands);
        private static bool IsHelpCommand(string userInput) => IsCommand(userInput, helpCommands);

        private static bool IsCommand(string userInput, params string[] commandsToMatch)
        {
            if (commandsToMatch?.Any() != true)
                return false;

            return userInput.In(commandsToMatch, (input, item) => input.Is(item));
        }

        private void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            commandCancelTokenSource.Cancel();
        }
    }
}
