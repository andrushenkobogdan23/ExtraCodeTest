using MongoDB.Driver;
using Shared.Common.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver.GridFS;

namespace Shared.Common.Mongo
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGridFSBucket _docs;
        private IGridFSBucket _pics;
        private readonly IMessageBus _bus;

        public UnitOfWork(IOptions<Settings> settings, IMessageBus bus) 
        {

            _bus = bus;

            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                var db = client.GetDatabase(settings.Value.Database);
                Repository = new GenericRepository(db);
            }
        }

        public IGenericRepository Repository { get; }
        public IGridFSBucket Docs
        {
            get
            {
                if (_docs == null)
                {
                    var gridFSBucketOptions = new GridFSBucketOptions()
                    {
                        BucketName = "docs",
                        ChunkSizeBytes = 1024 * 100, // 100 kB
                    };

                    _docs = new GridFSBucket(Repository.Db, gridFSBucketOptions);
                }
                return _docs;
            }
        }

        public IGridFSBucket Pics
        {
            get
            {
                if (_pics == null)
                {
                    var gridFSBucketOptions = new GridFSBucketOptions()
                    {
                        BucketName = "pics",
                        ChunkSizeBytes = 1024 * 10, // 10 kB
                    };

                    _pics = new GridFSBucket(Repository.Db, gridFSBucketOptions);
                }
                return _pics;
            }
        }

        public async Task PublishEventAsync<M>(M msg) where M : BusEvent
        {
            //await Task.FromResult(0);
            await _bus.PublishAsync(msg);
        }
    }
}
