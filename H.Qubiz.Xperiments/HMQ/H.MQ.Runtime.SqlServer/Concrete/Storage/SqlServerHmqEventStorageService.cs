using H.Necessaire.Dapper;
using H.Necessaire;
using System;
using System.Collections.Generic;
using System.Text;
using H.MQ.Abstractions;

namespace H.MQ.Runtime.SqlServer.Concrete.Storage
{
    internal partial class SqlServerHmqEventStorageService
    {
    }

    internal class HmqEventSqlEntry : SqlEntryBase, IGuidIdentity
    {
        public Guid ID { get; set; }
        public DateTime HappenedAt { get; set; }
        public string RaisedByJson { get; set; }
        public string RaisedByID { get; set; }


        public string Name { get; set; }

        public string Type { get; set; }

        public string Assembly { get; set; }


        public string AttributesJson { get; set; }


        public string DataJson { get; set; }
    }
}
