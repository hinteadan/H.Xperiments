using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Collections.Generic;
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

                await foreach(string mess in new InfiniteEnumerable<string>(async x => { await Task.Delay(Random.Shared.Next(500, 1500)); return x.ToString(); }, (m, x) => (x == 5).AsTask()))
                {
                    Log(mess);
                }
            }

            return OperationResult.Win();
        }

        async IAsyncEnumerable<string> DummyStream()
        {
            int index = 10;

            while (index >= 0)
            {
                index--;

                await Task.CompletedTask;
                yield return $"{index}";
            }
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
