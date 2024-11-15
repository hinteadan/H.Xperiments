using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal static class EnumerableBatchExtensions
    {
        public static async Task ForEachBatch<T>(this IEnumerable<T> enumerable, Func<T[], int, Task> onBatch, int batchSize = 10, Func<T, int, int, Task> onElement = null)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            if (enumerable is null)
                return;

            int currentBatchIndex = -1;
            int currentElementIndex = -1;
            T[] latestBatch = new T[batchSize];
            bool hasIncompleteLastBatch = false;

            foreach (T element in enumerable)
            {
                currentElementIndex++;
                currentBatchIndex = currentElementIndex / batchSize;
                bool isBatchEnd = (currentElementIndex + 1) % batchSize == 0;
                latestBatch[currentElementIndex % batchSize] = element;
                hasIncompleteLastBatch = true;

                if (onElement != null)
                {
                    await onElement.Invoke(element, currentElementIndex, currentBatchIndex);
                }

                if (isBatchEnd && onBatch != null)
                {
                    await onBatch.Invoke(latestBatch, currentBatchIndex);
                    Array.Clear(latestBatch, 0, latestBatch.Length);
                    hasIncompleteLastBatch = false;
                }
            }

            if (hasIncompleteLastBatch && onBatch != null)
            {
                T[] batch = new T[currentElementIndex % batchSize + 1];
                Array.Copy(latestBatch, batch, batch.Length);
                await onBatch.Invoke(batch, currentBatchIndex);
            }
        }

        public static async Task ForEachBatch<T>(this IAsyncEnumerable<T> enumerable, Func<T[], int, Task> onBatch, int batchSize = 10, Func<T, int, int, Task> onElement = null)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            if (enumerable is null)
                return;

            int currentBatchIndex = -1;
            int currentElementIndex = -1;
            T[] latestBatch = new T[batchSize];
            bool hasIncompleteLastBatch = false;

            IAsyncEnumerator<T> enumerator = enumerable.GetAsyncEnumerator();

            while (await enumerator.MoveNextAsync())
            {
                T element = enumerator.Current;
                currentElementIndex++;
                currentBatchIndex = currentElementIndex / batchSize;
                bool isBatchEnd = (currentElementIndex + 1) % batchSize == 0;
                latestBatch[currentElementIndex % batchSize] = element;
                hasIncompleteLastBatch = true;

                if (onElement != null)
                {
                    await onElement.Invoke(element, currentElementIndex, currentBatchIndex);
                }

                if (isBatchEnd && onBatch != null)
                {
                    await onBatch.Invoke(latestBatch, currentBatchIndex);
                    Array.Clear(latestBatch, 0, latestBatch.Length);
                    hasIncompleteLastBatch = false;
                }
            }

            if (hasIncompleteLastBatch && onBatch != null)
            {
                T[] batch = new T[currentElementIndex % batchSize + 1];
                Array.Copy(latestBatch, batch, batch.Length);
                await onBatch.Invoke(batch, currentBatchIndex);
            }
        }

        public static void ForEachBatch<T>(this IEnumerable<T> enumerable, Action<T[], int> onBatch, int batchSize = 10, Action<T, int, int> onElement = null)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            if (enumerable is null)
                return;

            int currentBatchIndex = -1;
            int currentElementIndex = -1;
            T[] latestBatch = new T[batchSize];
            bool hasIncompleteLastBatch = false;

            foreach (T element in enumerable)
            {
                currentElementIndex++;
                currentBatchIndex = currentElementIndex / batchSize;
                bool isBatchEnd = (currentElementIndex + 1) % batchSize == 0;
                latestBatch[currentElementIndex % batchSize] = element;
                hasIncompleteLastBatch = true;

                if (onElement != null)
                {
                    onElement.Invoke(element, currentElementIndex, currentBatchIndex);
                }

                if (isBatchEnd && onBatch != null)
                {
                    onBatch.Invoke(latestBatch, currentBatchIndex);
                    Array.Clear(latestBatch, 0, latestBatch.Length);
                    hasIncompleteLastBatch = false;
                }
            }

            if (hasIncompleteLastBatch && onBatch != null)
            {
                T[] batch = new T[currentElementIndex % batchSize + 1];
                Array.Copy(latestBatch, batch, batch.Length);
                onBatch.Invoke(batch, currentBatchIndex);
            }
        }

        public static async Task ForEachBatch<T>(this IAsyncEnumerable<T> enumerable, Action<T[], int> onBatch, int batchSize = 10, Action<T, int, int> onElement = null)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            if (enumerable is null)
                return;

            int currentBatchIndex = -1;
            int currentElementIndex = -1;
            T[] latestBatch = new T[batchSize];
            bool hasIncompleteLastBatch = false;

            IAsyncEnumerator<T> enumerator = enumerable.GetAsyncEnumerator();

            while (await enumerator.MoveNextAsync())
            {
                T element = enumerator.Current;
                currentElementIndex++;
                currentBatchIndex = currentElementIndex / batchSize;
                bool isBatchEnd = (currentElementIndex + 1) % batchSize == 0;
                latestBatch[currentElementIndex % batchSize] = element;
                hasIncompleteLastBatch = true;

                if (onElement != null)
                {
                    onElement.Invoke(element, currentElementIndex, currentBatchIndex);
                }

                if (isBatchEnd && onBatch != null)
                {
                    onBatch.Invoke(latestBatch, currentBatchIndex);
                    Array.Clear(latestBatch, 0, latestBatch.Length);
                    hasIncompleteLastBatch = false;
                }
            }

            if (hasIncompleteLastBatch && onBatch != null)
            {
                T[] batch = new T[currentElementIndex % batchSize + 1];
                Array.Copy(latestBatch, batch, batch.Length);
                onBatch.Invoke(batch, currentBatchIndex);
            }
        }
    }
}
