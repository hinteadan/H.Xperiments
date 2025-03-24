using Couchbase.Lite;
using Couchbase.Lite.Query;
using H.Necessaire;
using H.Necessaire.CLI.Commands;
using H.Necessaire.Serialization;
using H.Xperiments.Couchbase.BLL;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace H.Xperiments.Couchbase
{
    [Alias("couch")]
    class CouchbaseCommand : CommandBase
    {
        public override async Task<OperationResult> Run()
        {
            var cb = new CouchbaseInteractor("dev-play-db");

            //var data = new DummyData();


            DataBin dataBin
                = new DataBinMeta { Name = "Test", Format = WellKnownDataBinFormat.PlainTextUTF8 }
                .ToBin(meta =>
                {
                    return $"Test Content @ {DateTime.UtcNow}".ToStream().ToDataBinStream().AsTask();
                });



            using (CouchbaseOperations scope = cb.NewOperationScope((nameof(DummyData))))
            {
                var blobSaveResult = await scope.SaveBlob(dataBin);
                DataBin blob = (await scope.StreamAllBlobs()).ThrowOnFailOrReturn().FirstOrDefault();
                await scope.DeleteBlob(blob.ID);
            }

            return OperationResult.Win();
        }
    }

    class DummyData : EphemeralTypeBase, IGuidIdentity
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = "Hin";
        public string LastName { get; set; } = "Tee";
    }
}
