using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OngResgisterApi.Models;

public class MongoBaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime CreatedTime { get; } = DateTime.Now!;

    public DateTime UpdatedTime { get; set; } = DateTime.Now!;
}


