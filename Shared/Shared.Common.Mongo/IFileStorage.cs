using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Shared.Common.Mongo
{
    public interface IFileStorage
    {
        Task<ObjectId> AddAsync(Stream fs, string fileName);
        Task DeleteAsync(GridFSFileInfo info);
        Task<GridFSFileInfo> GetInfoAsync(ObjectId id);
        Task<Stream> LoadAsync(ObjectId id);
        Task ReplaceAsync(Stream fs, string fileName, ObjectId existingId);
    }
}