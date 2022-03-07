using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSI5112BackEndApi.Models;

[BsonIgnoreExtraElements]
public class ShippingAddress
{
    [BsonId] // Annotated with BsonId to make this property the document's primary key
    [BsonRepresentation(BsonType.ObjectId)] // to allow passing the parameter as type string instead of an ObjectId structure
    public string? shipping_address_id { get; set; } = null!;

    public string address { get; set; } = null!;

    public string city { get; set; } = null!;

    public string state { get; set; } = null!;

    public string zipcode { get; set; } = null!;

    public string country { get; set; } = null!;
}