using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Threading.Tasks;

namespace Shared.Common.Mongo
{
    public class FileStorage : IFileStorage
    {
        private IGridFSBucket _fs = null;

        public FileStorage(IMongoDatabase db)
        {
            var opt = new GridFSBucketOptions();

            _fs = new GridFSBucket(db, opt);


        }

        /// <summary>
        /// Add new file to the GridFS
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<ObjectId> AddAsync(Stream fs, string fileName)
        {
            var id = await _fs.UploadFromStreamAsync(fileName, fs);

            return id;
        }

        /// <summary>
        /// load file into memory stream
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Memory stream</returns>
        public async Task<Stream> LoadAsync(ObjectId id)
        {
            var stream = new MemoryStream();

            await _fs.DownloadToStreamAsync(id, stream);

            return stream;
        }

        /// <summary>
        /// Replace existing file with the new one
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="fileName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ReplaceAsync(Stream fs, string fileName, ObjectId existingId)
        {
            {
                await _fs.UploadFromStreamAsync(
                    fileName,
                    fs,
                    new GridFSUploadOptions { Metadata = new BsonDocument("_id", existingId) });
            }
        }


        public async Task DeleteAsync(GridFSFileInfo info)
        {
            await _fs.DeleteAsync(info.Id);
        }


        public async Task<GridFSFileInfo> GetInfoAsync(ObjectId id)
        {
            // создаем фильтр для поиска
            var filter = Builders<GridFSFileInfo>.Filter.Eq(info => info.Id, id);
            
            // находим все файлы
            var fileInfos = await _fs.FindAsync(filter);
            
            // получаем первый файл
            var fileInfo = fileInfos.FirstOrDefault();

            return fileInfo;
        }
    }
}
