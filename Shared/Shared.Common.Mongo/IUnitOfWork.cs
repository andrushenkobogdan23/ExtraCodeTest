using MongoDB.Driver.GridFS;
using System.Threading.Tasks;

namespace Shared.Common.Mongo
{
    public interface IUnitOfWork
    {
        IGenericRepository Repository { get; }

        IGridFSBucket Docs { get; }
        IGridFSBucket Pics { get; }

        Task PublishEventAsync<M>(M msg) where M : BusEvent;
    }
}
