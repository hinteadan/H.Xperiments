using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using Spectre.Console;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console.Json;
using System.Text;
using H.Necessaire.Serialization;
using System;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    [ID("ui")]
    internal class UiCliCommand : CommandBase
    {
        public override async Task<OperationResult> Run()
        {
            await AnsiConsole
                .Progress()
                .Columns(new ProgressColumn[]
    {
        new TaskDescriptionColumn(),    // Task description
        new ProgressBarColumn(),        // Progress bar
        new PercentageColumn(),         // Percentage
        new RemainingTimeColumn(),      // Remaining time
        new SpinnerColumn(),            // Spinner
    })
                .StartAsync(async ctx => {

                    var t1 = ctx.AddTask("[yellow]Task 1[/]");
                    var t2 = ctx.AddTask("[green]Task 2[/]");

                    while (!ctx.IsFinished)
                    {
                        await Task.Delay(100);
                        t1.Increment(.1);
                        t2.Increment(.13);
                    }
                });

            Console.WriteLine("Done");


            return OperationResult.Win();
        }
    }
}
