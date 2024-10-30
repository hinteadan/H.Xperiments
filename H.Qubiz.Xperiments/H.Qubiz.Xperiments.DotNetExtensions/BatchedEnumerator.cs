using System;
using System.Collections;
using System.Collections.Generic;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal class BatchedEnumerator<T> : IEnumerator<IEnumerable<T>>
    {
        int currentSourceIndex = -1;
        int currentBatchIndex = -1;
        private readonly int batchSize;
        private readonly IEnumerable<T> sourceEnumerable;
        public BatchedEnumerator(IEnumerable<T> sourceEnumerable, int batchSize)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            this.batchSize = batchSize;
            this.sourceEnumerable = sourceEnumerable;
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
            
        }

        public void Reset()
        {
            currentBatchIndex = -1;
            currentSourceIndex = -1;
            Current = null;
        }

        bool HasCurrentBatchEnded()
        {
            return currentSourceIndex == -1 || currentSourceIndex % batchSize == 0;
        }
    }
}
