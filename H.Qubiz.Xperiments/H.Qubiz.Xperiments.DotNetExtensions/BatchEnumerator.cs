using System;
using System.Collections;
using System.Collections.Generic;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal class BatchEnumerator<T> : IEnumerator<T>
    {
        private readonly int batchIndex;
        private readonly int batchSize;
        private readonly IEnumerator<T> sourceEnumerator;
        public BatchEnumerator(IEnumerator<T> sourceEnumerator, int batchIndex, int batchSize)
        {
            this.sourceEnumerator = sourceEnumerator;
            this.batchIndex = batchIndex;
            this.batchSize = batchSize;
        }

        public T Current => throw new NotImplementedException();

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
