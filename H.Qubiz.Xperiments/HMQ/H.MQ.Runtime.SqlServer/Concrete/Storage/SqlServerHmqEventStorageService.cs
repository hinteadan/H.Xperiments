using Dapper;
using H.MQ.Abstractions;
using H.Necessaire;
using H.Necessaire.Dapper;
using H.Necessaire.Runtime.SqlServer;
using H.Necessaire.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H.MQ.Runtime.SqlServer.Concrete.Storage
{
    internal partial class SqlServerHmqEventStorageService : DapperStorageServiceBase<Guid, HmqEvent, HmqEventSqlEntry, HmqEventFilter>
    {
        #region Construct
        public SqlServerHmqEventStorageService() : base(connectionString: null, tableName: table, databaseName: "H.Necessaire.HMQ.Core") { }
        protected override Task<SqlMigration[]> GetAllMigrations() => sqlMigrations.AsTask();
        #endregion
        protected override ISqlFilterCriteria[] ApplyFilter(HmqEventFilter filter, DynamicParameters sqlParams)
        {
            List<ISqlFilterCriteria> result = new List<ISqlFilterCriteria>();



            return result.ToArray();
        }

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

    internal class HmqEventSqlEntityMapper : SqlEntityMapperBase<HmqEvent, HmqEventSqlEntry>
    {
        static HmqEventSqlEntityMapper() => new HmqEventSqlEntityMapper().RegisterMapper();

        public override HmqEventSqlEntry MapEntityToSql(HmqEvent entity)
        {
            return
                base
                .MapEntityToSql(entity)
                .And(x =>
                {
                    x.RaisedByJson = entity.RaisedBy?.ToJsonObject();
                    x.AttributesJson = entity.Attributes?.ToJsonArray();
                    x.DataJson = entity.Data?.ToJsonObject();
                })
                ;
        }

        public override HmqEvent MapSqlToEntity(HmqEventSqlEntry sqlEntity)
        {
            return
                base
                .MapSqlToEntity(sqlEntity)
                .And(x =>
                {
                    x.RaisedBy = sqlEntity.RaisedByJson?.JsonToObject<HmqActorIdentity>();
                    x.Attributes = sqlEntity.AttributesJson?.DeserializeToNotes();
                    x.Data = sqlEntity.DataJson?.JsonToObject<object>();
                })
                ;
        }
    }
}
