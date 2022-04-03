using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CSI5112BackEndApi.Models;

public class SalesOrderEcho
{
    public SalesOrder salesOrder{ get; set; } = null!;
    public ShippingAddress merchantAddress{ get; set; } = null!;
    public ShippingAddress customerAddress{ get; set; } = null!;
}