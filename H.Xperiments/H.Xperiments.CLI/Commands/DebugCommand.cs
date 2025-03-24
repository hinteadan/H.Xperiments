using H.Necessaire;
using H.Necessaire.CLI.Commands;
using H.Xperiments.DotNetExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace H.Xperiments.CLI.Commands
{
    internal class DebugCommand : CommandBase
    {
        class DummyActivationTester
        {

            //public DummyActivationTester()
            //{

            //}
            //public DummyActivationTester(int a, DateTime dateTime)
            //{

            //}
        }

        public override async Task<OperationResult> Run()
        {
            Log("Debugging...");
            using (new TimeMeasurement(x => Log($"DONE Debugging in {x}")))
            {
                await Task.Delay(0);

                var x = typeof(List<int>).GetInterface(nameof(IEnumerable));
                var xx = typeof(DateTime).GetInterface(nameof(IEnumerable));

                var a = typeof(DummyActivationTester).GetConstructor([]);
                var b = typeof(DummyActivationTester).GetConstructor([typeof(int)]);
                var c = typeof(DummyActivationTester).GetConstructor([typeof(int), typeof(DateTime)]);

                var ai = a.Invoke([]);

                Activator.CreateInstance(typeof(DummyActivationTester), 1, DateTime.UtcNow);

                return OperationResult.Win();

                CallContext.SetData("A", 1);
                CallContext.SetData("B", 2);

                var xxx = ExecutionContext.Capture();
                var xxy = SynchronizationContext.Current;

                var user = Thread.CurrentPrincipal;

                ThreadPool.GetMaxThreads(out int wt, out int iot);
                ThreadPool.GetAvailableThreads(out int wt2, out int iot2);

                await Enumerable.Range(0, 13).ForEachBatch(
                    onBatch: async (batch, batchIndex, c) =>
                {
                },
                    onElement: async (x, index, batchIndex, c) =>
                {
                }, batchSize: 50);

                await NewRandomInts().ForEachBatch((batch, batchIndex, c) =>
                {
                });

                await foreach (int value in NewRandomInts())
                {
                    Log($"{value}");
                }

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
