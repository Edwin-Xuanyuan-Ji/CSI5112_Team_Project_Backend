using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSI5112BackEndApi.Models;

[BsonIgnoreExtraElements]
public class Answer
{
    [BsonId] // Annotated with BsonId to make this property the document's primary key
    [BsonRepresentation(BsonType.ObjectId)] // to allow passing the parameter as type string instead of an ObjectId structure
    public string? answer_id { get; set; } = null!;

    public string question_id { get; set; } = null!;

    public string role { get; set; } = null!;

    public string role_id { get; set; } = null!;

    public string answer { get; set; } = null!;

    public DateTime date { get; set; }

}