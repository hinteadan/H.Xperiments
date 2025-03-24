using Couchbase.Lite.Query;
using H.Necessaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace H.Xperiments.Couchbase.BLL.Querying
{
    public static class CouchbaseQueryWizard
    {
        public static SelectOperationResult<T> SelectAll<T>(this CouchbaseOperationScope operationScope)
        {
            return
                new SelectOperationResult<T>(DataSource.Collection(operationScope.Collection), CouchbaseLinqExpressionInterpreter.Instance.SelectAll());
        }

        public static SelectOperationResult<T> SelectCount<T>(this CouchbaseOperationScope operationScope)
        {
            return
                new SelectOperationResult<T>(DataSource.Collection(operationScope.Collection), CouchbaseLinqExpressionInterpreter.Instance.SelectCount());
        }

        public static SelectOperationResult<T> SelectCustom<T>(this CouchbaseOperationScope operationScope, params Expression<Func<T, object>>[] selectors)
        {
            return
                new SelectOperationResult<T>(DataSource.Collection(operationScope.Collection), CouchbaseLinqExpressionInterpreter.Instance.Select(selectors));
        }
    }

}
