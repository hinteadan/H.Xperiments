using H.Necessaire;
using H.Necessaire.Runtime.CLI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using H.Qubiz.Xperiments.DotNetExtensions;

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

                await Enumerable.Range(0, 10).ForEachBatch(
                    onBatch: async (batch, batchIndex) =>
                {
                },
                    onElement: async (x, index, batchIndex) =>
                {
                });

                //await foreach(int value in NewRandomInts())
                //{
                //    Log($"{value}");
                //}

                Log("Debug Command");
            }

            return OperationResult.Win();
        }

        IAsyncEnumerable<int> NewRandomInts()
        {
            return new InfiniteAsyncEnumerable<int>(async index =>
            {
                await Task.Delay(Random.Shared.Next(500, 3500));
                return Random.Shared.Next(int.MinValue, int.MaxValue);
            });
        }


        class InfiniteAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            readonly Func<int, Task<T>> valueFactory;
            public InfiniteAsyncEnumerable(Func<int, Task<T>> valueFactory)
            {
                this.valueFactory = valueFactory;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new InfiniteAsyncEnumerator(valueFactory);
            }



            class InfiniteAsyncEnumerator : IAsyncEnumerator<T>
            {
                int index = -1;
                readonly Func<int, Task<T>> valueFactory;
                public InfiniteAsyncEnumerator(Func<int, Task<T>> valueFactory)
                {
                    this.valueFactory = valueFactory;
                }

                public T Current { get; private set; }

                public ValueTask DisposeAsync() => ValueTask.CompletedTask;

                public async ValueTask<bool> MoveNextAsync()
                {
                    Current = await valueFactory(++index);

                    return true;
                }
            }
        }
    }
}
