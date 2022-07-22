using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CSI5112BackEndApi.Models;

[BsonIgnoreExtraElements]
public class CartProduct
{
    [BsonId] // Annotated with BsonId to make this property the document's primary key
    [BsonRepresentation(BsonType.ObjectId)] // to allow passing the parameter as type string instead of an ObjectId structure
    public string? item_id { get; set; } = null!;

    public string? product_id { get; set; } = null!;

    public string category { get; set; } = null!;

    public string description { get; set; } = null!;

    public string manufacturer { get; set; } = null!;

    public string name { get; set; } = null!;

    public int? price { get; set; } = null!;

    public string owner { get; set; } = null!;

    public string owner_id { get; set; } = null!;

    public string image { get; set; } = null!;

    public string image_type { get; set; } = null!;

    public DateTime date { get; set; }

    public int quantity {get; set;}
}
