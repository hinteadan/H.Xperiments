using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace H.Qubiz.Xperiments.CLI.Commands
{
    internal class DebugCommand : CommandBase
    {
        public override async Task<OperationResult> Run()
        {
            Log("Debugging...");
            using (new TimeMeasurement(x => Log($"DONE Debugging in {x}")))
            {
                await Task.Delay(0);
                Log("Debug Command");

                var ips = await Dns.GetHostAddressesAsync(Dns.GetHostName());

                //Ping ping = new Ping();

                //await foreach(var mess in new InfiniteEnumerable<string>(async x => {
                //    if (x < 2)
                //        return null;
                //    var pingReply = await ping.SendPingAsync();
                //    return x.ToString();
                //}, (m, x) => (x >= 255).AsTask()))
                //{
                //    Log(mess);
                //}
            }

            return OperationResult.Win();
        }

        class InfiniteEnumerator<T> : IAsyncEnumerator<T>
        {
            readonly Func<int, Task<T>> valueFactory;
            readonly Func<T, int, Task<bool>> endOfStreamChecker;
            int currentIteration = -1;
            public InfiniteEnumerator(Func<int, Task<T>> valueFactory, Func<T, int, Task<bool>> endOfStreamChecker)
            {
                this.valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
                this.endOfStreamChecker = endOfStreamChecker ?? throw new ArgumentNullException(nameof(endOfStreamChecker));
            }

            public T Current { get; private set; }

            public ValueTask DisposeAsync() => ValueTask.CompletedTask;

            public async ValueTask<bool> MoveNextAsync()
            {
                currentIteration++;

                T value = await valueFactory(currentIteration);
                bool isEndOfStream = await endOfStreamChecker(value, currentIteration);
                if (isEndOfStream)
                    return false;

                Current = value;

                return true;
            }
        }

        class InfiniteEnumerable<T> : IAsyncEnumerable<T>
        {
            readonly Func<int, Task<T>> valueFactory;
            readonly Func<T, int, Task<bool>> endOfStreamChecker;
            public InfiniteEnumerable(Func<int, Task<T>> valueFactory, Func<T, int, Task<bool>> endOfStreamChecker)
            {
                this.valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
                this.endOfStreamChecker = endOfStreamChecker ?? throw new ArgumentNullException(nameof(endOfStreamChecker));
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new InfiniteEnumerator<T>(valueFactory, endOfStreamChecker);
            }
        }
    }
}
