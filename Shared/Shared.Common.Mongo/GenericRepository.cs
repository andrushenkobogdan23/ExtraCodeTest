using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Common.Interfaces;

namespace Shared.Common.Mongo
{

    public class GenericRepository : IGenericRepository
    {
        //private readonly IMongoDatabase _db = null;

        public GenericRepository(IMongoDatabase db)
        {
            Db = db;
        }

        public IMongoDatabase Db { get; private set; }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Db.GetCollection<T>(name);
        }


        public FilterDefinition<T> GetFilterById<T>(ObjectId id) {
            return Builders<T>.Filter.Eq("_id", id);
        }

        public UpdateDefinition<T> GetUpdateState<T>(string field, object doc)
        {
            return Builders<T>.Update.Set(field, doc);
        }

        public UpdateDefinition<T> GetPushState<T>(string field, object doc)
        {
            return Builders<T>.Update.Push(field, doc);
        }

        public UpdateDefinition<T> GetPullState<T>(string field, object doc)
        {
            return Builders<T>.Update.Pull(field, doc);
        }


        /// <summary>
        /// get all collection items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAll<T>() where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();
                return await col.Find(x => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        /// <summary>
        /// find items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Find<T>(FilterDefinition<T> filter) where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();

                var items = await col.Find(filter).Limit(1000).ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }   
        
                
        public async Task<IEnumerable<T>> Find<T>(string colName, FilterDefinition<T> filter)
        {
            try
            {
                var col = GetCollection<T>(colName);

                var items = await col.Find(filter).Limit(1000).ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        /// <summary>
        /// get item by _id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Get<T, K>(K id) where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();
                var filter = GetFilterById<T, K>(id);

                return await col.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<T> Get<T>(ObjectId id, string colName) where T : class
        {
            try
            {
                var col = Db.GetCollection<T>(colName);
                var filter = Builders<T>.Filter.Eq("_id", id);

                return await col.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }


        public async Task Add<T>(T item) where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();

                await col.InsertOneAsync(item);

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddMany<T>(IEnumerable<T> items) where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();
                await col.InsertManyAsync(items);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task PushChild<T, C>(ObjectId id, string prop, C child) where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();
                var filter = GetFilterById<T, ObjectId>(id);
                var push = Builders<T>.Update.Push(prop, child);

                await col.UpdateOneAsync(filter, push, new UpdateOptions() { IsUpsert = true });
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> Remove<T, K>(K id) where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();
                var filter = GetFilterById<T, K>(id);
                var actionResult = await col.DeleteOneAsync(filter);

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> Update<T, K>(K id, T item) where T : MongoDomainModel
        {
            try
            {
                var col = GetTypeCollection<T>();
                var filter = GetFilterById<T, K>(id);

                var actionResult = await col.ReplaceOneAsync(filter, item, new UpdateOptions { IsUpsert = true });

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> Update<T>(ObjectId id, UpdateDefinition<T> update)
        {
            try
            {
                var col = GetTypeCollection<T>();
                var filter = GetFilterById<T, ObjectId>(id);
                var actionResult = await col.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private FilterDefinition<T> GetFilterById<T, K>(K id)
        {
            return Builders<T>.Filter.Eq(nameof(MongoDomainModel.Id), id);
        }


        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id.ToString(), out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public IMongoCollection<T> GetTypeCollection<T>()
        {
            var colName = typeof(T).Name;
            return Db.GetCollection<T>(colName);
        }

        public Task PushChild<T, Y>(string id, string prop, Y child) where T : MongoDomainModel
        {
            throw new NotImplementedException();
        }
    }
}