using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Common.Mongo
{
    public abstract class MongoDomainModel //: TypedDomainModel<ObjectId>
    {
        [BsonId]//, JsonConverter(typeof(ObjectIdConverter))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
