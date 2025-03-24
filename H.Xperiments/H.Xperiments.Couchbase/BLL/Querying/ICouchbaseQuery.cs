using Couchbase.Lite.Query;

namespace H.Xperiments.Couchbase.BLL.Querying
{
    public interface ICouchbaseQuery
    {
        IQuery ToQuery();
        IQuery ToCountQuery();
    }
}
