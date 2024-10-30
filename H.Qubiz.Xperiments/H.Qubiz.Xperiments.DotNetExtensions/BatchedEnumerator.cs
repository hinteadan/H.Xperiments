using System;
using System.Collections;
using System.Collections.Generic;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal class BatchedEnumerator<T> : IEnumerator<IEnumerable<T>>
    {
        int currentSourceIndex = -1;
        int currentBatchIndex = -1;
        bool hasMoreData = true;
        private readonly int batchSize;
        private readonly IEnumerable<T> sourceEnumerable;
        private readonly IEnumerator<T> sourceEnumerator;
        public BatchedEnumerator(IEnumerable<T> sourceEnumerable, int batchSize)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            this.batchSize = batchSize;
            this.sourceEnumerable = sourceEnumerable;
            this.sourceEnumerator = sourceEnumerable.GetEnumerator();
        }


        public IEnumerable<T> Current { get; private set; }
        object IEnumerator.Current => Current;
        public void Dispose()
        {
            currentBatchIndex = -1;
            currentSourceIndex = -1;
            Current = null;
        }



        public bool MoveNext()
        {
            if (!hasMoreData)
                return false;

            if (HasCurrentBatchEnded())
            {
                currentSourceIndex++;
                currentBatchIndex++;
                Current = new BatchEnumerable<T>(new BatchEnumerator<T>(sourceEnumerator, currentBatchIndex, batchSize, OnMoveNext));
            }

            return true;
        }

        public void Reset()
        {
            currentBatchIndex = -1;
            currentSourceIndex = -1;
            Current = null;
        }

        bool HasCurrentBatchEnded()
        {
            return (currentSourceIndex + 1) % batchSize == 0;
        }

        void OnMoveNext(int indexInCurrentBatch, bool hasMoreData)
        {
            this.hasMoreData = hasMoreData;
            if (!hasMoreData)
            {
                return;
            }

            currentSourceIndex++;
        }
    }
}
