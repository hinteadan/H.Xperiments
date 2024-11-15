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
        private readonly Action<int, bool> onMoveNext;
        public BatchEnumerator(IEnumerator<T> sourceEnumerator, int batchIndex, int batchSize, Action<int, bool> onMoveNext)
        {
            this.sourceEnumerator = sourceEnumerator;
            this.batchIndex = batchIndex;
            this.batchSize = batchSize;
            this.onMoveNext = onMoveNext;
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

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
