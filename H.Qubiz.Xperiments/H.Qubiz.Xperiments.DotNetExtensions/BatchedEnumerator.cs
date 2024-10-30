using System;
using System.Collections;
using System.Collections.Generic;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal class BatchedEnumerator<T> : IEnumerator<IEnumerable<T>>
    {
        int sourceIndex = -1;
        int batchIndex = -1;
        private readonly int batchSize;
        private readonly IEnumerable<T> sourceEnumerable;
        public BatchedEnumerator(IEnumerable<T> sourceEnumerable, int batchSize)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Batch Size cannot be equal or less than zero for obvious reasons");

            this.batchSize = batchSize;
            this.sourceEnumerable = sourceEnumerable;
        }


        public IEnumerable<T> Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
