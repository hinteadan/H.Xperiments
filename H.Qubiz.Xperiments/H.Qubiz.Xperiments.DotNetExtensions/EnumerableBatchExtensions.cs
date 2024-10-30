using System.Collections.Generic;

namespace H.Qubiz.Xperiments.DotNetExtensions
{
    internal static class EnumerableBatchExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> enumerable, int batchSize = 10)
        {
            if (enumerable is null)
                return null;

            return new BatchedEnumerable<T>(enumerable, batchSize);
        }
    }
}
