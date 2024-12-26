using H.Necessaire;
using H.Necessaire.CLI.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace H.Xperiments.DotNetStuff
{
    [Alias("th")]
    internal class ThreadingCommand : CommandBase
    {
        const int timersCount = 10;
        static readonly TimeSpan timerInterval = TimeSpan.FromMilliseconds(15);
        static readonly TimeSpan timeToRun = TimeSpan.FromSeconds(5);
        readonly System.Threading.CancellationTokenSource cancellationTokenSource = new ();
        Timer timer = new Timer(timerInterval) { AutoReset = false };
        public override async Task<OperationResult> Run()
        {
            Log("Running DotNet Threading Command...");
            using (new TimeMeasurement(x => Log($"DONE Running DotNet Threading Command in {x}")))
            {
                await Task.Delay(0);

                timer.Start();
                timer.Elapsed += Timer_Elapsed;

                await Task.Delay(timeToRun);

                cancellationTokenSource.Cancel();

                timer.Elapsed -= Timer_Elapsed;
                timer.Stop();
            }

            return OperationResult.Win();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            Log($"{System.Threading.Thread.CurrentThread.ManagedThreadId} - {System.Threading.Thread.CurrentThread.Name}");

            timer.Stop();
            //timer.Dispose();

            timer = new Timer(timerInterval) { AutoReset = false };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
    }
}
