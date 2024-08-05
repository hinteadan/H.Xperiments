using H.MQ.Abstractions;
using H.Necessaire;
using H.Necessaire.Dapper;

namespace H.MQ.Runtime.SqlServer.Concrete.Storage.Migrations
{
    internal partial class SqlServerHmqEventStorageService
    {
        const string tableName = "H.Necessaire.HMQ.HmqEvent";

        static readonly SqlMigration[] sqlMigrations = new SqlMigration[] {
            new SqlMigration
            {
                ResourceIdentifier = "HmqEvent",
                VersionNumber = new VersionNumber(1, 0),
                SqlCommand = $@"
CREATE TABLE [dbo].[{tableName}]
(
    [{nameof(HmqEventSqlEntry.ID)}] [uniqueidentifier] NOT NULL,
    [{nameof(HmqEventSqlEntry.HappenedAt)}] [datetime2](7) NOT NULL,
    [{nameof(HmqEventSqlEntry.RaisedByJson)}] [nvarchar](MAX) NULL,
    [{nameof(HmqEventSqlEntry.RaisedByID)}] [nvarchar](450) NULL,
    [{nameof(HmqEventSqlEntry.Name)}] [nvarchar](450) NULL,
    [{nameof(HmqEventSqlEntry.Type)}] [nvarchar](450) NULL,
    [{nameof(HmqEventSqlEntry.Assembly)}] [nvarchar](450) NULL,
    [{nameof(HmqEventSqlEntry.AttributesJson)}] [nvarchar](MAX) NULL,
    [{nameof(HmqEventSqlEntry.DataJson)}] [nvarchar](MAX) NULL,

    {nameof(HmqEvent.ID).PrintColumnAsPrimaryKeyConstraintSqlScriptOn(tableName)}
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

{nameof(HmqEventSqlEntry.HappenedAt).PrintColumnIndexCreationSqlScriptOn(tableName)}
{nameof(HmqEventSqlEntry.RaisedByID).PrintColumnIndexCreationSqlScriptOn(tableName)}
{nameof(HmqEventSqlEntry.Name).PrintColumnIndexCreationSqlScriptOn(tableName)}
{nameof(HmqEventSqlEntry.Type).PrintColumnIndexCreationSqlScriptOn(tableName)}
{nameof(HmqEventSqlEntry.Assembly).PrintColumnIndexCreationSqlScriptOn(tableName)}
",
            }
        };
    }
}
