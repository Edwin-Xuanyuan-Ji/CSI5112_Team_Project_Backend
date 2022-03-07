using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSI5112BackEndApi.Models;

[BsonIgnoreExtraElements]
public class Question
{
    [BsonId] // Annotated with BsonId to make this property the document's primary key
    [BsonRepresentation(BsonType.ObjectId)] // to allow passing the parameter as type string instead of an ObjectId structure
    public string? question_id { get; set; } = null!;

    public string customer_id { get; set; } = null!;

    public string date { get; set; } = null!;

    public string question { get; set; } = null!;

    public string product_id { get; set; } = null!;

    public string[] answer_list { get; set; } = null!;

}