using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSI5112BackEndApi.Models;

[BsonIgnoreExtraElements]
public class PlaceOrdersFrontendRequire
{

    public string customer_id { get; set; } = null!;

    public string item_id{ get; set; } = null!;

    public string shipping_address_id { get; set; } = null!;

    public int? quantity { get; set; } = null!;

    public string product_id { get; set; } = null!;

    public DateTime date { get; set; }
}