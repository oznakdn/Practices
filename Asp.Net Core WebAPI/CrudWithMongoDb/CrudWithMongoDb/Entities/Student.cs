using MongoDB.Bson.Serialization.Attributes;

namespace CrudWithMongoDb.Entities
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;


        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("graduated")]
        public bool IsGraduated { get; set; }

        [BsonElement("courses")]
        public string[]? Courses { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; } = string.Empty;

        [BsonElement("age")]
        public int Age { get; set; }

    }
}
