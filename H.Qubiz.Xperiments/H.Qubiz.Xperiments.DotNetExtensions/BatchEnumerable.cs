using System.Collections;
using System.Collections.Generic;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal class BatchEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerator<T> sourceEnumerator;
        public BatchEnumerable(IEnumerator<T> sourceEnumerator)
        {
            this.sourceEnumerator = sourceEnumerator;
        }

        public IEnumerator<T> GetEnumerator() => sourceEnumerator;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
