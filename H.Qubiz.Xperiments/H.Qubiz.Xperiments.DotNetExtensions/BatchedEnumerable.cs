using System;
using System.Collections;
using System.Collections.Generic;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal class BatchedEnumerable<T> : IEnumerable<IEnumerable<T>>
    {
        private readonly int batchSize;
        private readonly IEnumerable<T> sourceEnumerable;
        public BatchedEnumerable(IEnumerable<T> sourceEnumerable, int batchSize)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            this.batchSize = batchSize;
            this.sourceEnumerable = sourceEnumerable ?? throw new ArgumentNullException(nameof(sourceEnumerable), "Source enumrable cannot be null");
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return new BatchedEnumerator<T>(sourceEnumerable, batchSize);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
