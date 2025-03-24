using System;
using System.Collections.Generic;
using System.Text;

namespace H.Xperiments.Couchbase.BLL.Abstract
{
    abstract class CouchbaseResourceBase
    {
        readonly string databaseName;
        readonly string databaseFolderPath;
        readonly string scopeName;
        readonly string collectionName;
        protected CouchbaseResourceBase(string databaseName, string databaseFolderPath, string scopeName, string collectionName)
        {
            this.databaseName = databaseName;
            this.databaseFolderPath = databaseFolderPath;
        }
    }
}
