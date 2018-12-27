using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Common.Mongo
{


    public interface IGenericRepository
    {
        Task<IEnumerable<T>> GetAll<T>() where T: MongoDomainModel;

        Task<T> Get<T, K>(K id) where T : MongoDomainModel;

        Task<T> Get<T>(ObjectId id, string colName) where T : class;

        // add new document
        Task Add<T>(T item) where T : MongoDomainModel;
        
        // add new document colection
        Task AddMany<T>(IEnumerable<T> items) where T : MongoDomainModel;

        // add a new comment for the documnet
        Task PushChild<T, C>(ObjectId id, string prop, C child) where T : MongoDomainModel;

        // remove a single document
        Task<bool> Remove<T, K>(K id) where T : MongoDomainModel;

        // update just a single document
        Task<bool> Update<T, K>(K id, T item) where T : MongoDomainModel;

        // update doc by _id and prepared update statement
        Task<bool> Update<T>(ObjectId id, UpdateDefinition<T> update);

        // search documents by condition
        Task<IEnumerable<T>> Find<T>(FilterDefinition<T> filter) where T : MongoDomainModel;

        Task<IEnumerable<T>> Find<T>(string colName, FilterDefinition<T> filter);

        IMongoDatabase Db { get; }

        IMongoCollection<T> GetCollection<T>(string name);

        FilterDefinition<T> GetFilterById<T>(ObjectId id);

        UpdateDefinition<T> GetUpdateState<T>(string field, object doc);

        UpdateDefinition<T> GetPushState<T>(string field, object doc);

        UpdateDefinition<T> GetPullState<T>(string field, object doc);


    }
}