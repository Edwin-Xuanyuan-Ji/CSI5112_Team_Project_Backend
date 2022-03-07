using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class ShippingAddressService
{
    private readonly IMongoCollection<ShippingAddress> _shippingAddressCollection;

    public ShippingAddressService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _shippingAddressCollection = mongoDatabase.GetCollection<ShippingAddress>(
            csi5112BackEndDataBaseSettings.Value.ShippingAddressCollectionName);
    }

    public async Task<List<ShippingAddress>> GetAllShippingAddress() =>
        await _shippingAddressCollection.Find(_ => true).ToListAsync();

    public async Task<ShippingAddress?> GetShippingAddressByID(string id) =>
        await _shippingAddressCollection.Find(x => x.shipping_address_id == id).FirstOrDefaultAsync();

    public async Task CreateNewShippingAddress(ShippingAddress newShippingAddress) =>
        await _shippingAddressCollection.InsertOneAsync(newShippingAddress);

    public async Task UpdateShippingAddress(string id, ShippingAddress updatedShippingAddress) =>
        await _shippingAddressCollection.ReplaceOneAsync(x => x.shipping_address_id == id, updatedShippingAddress);

    public async Task RemoveShippingAddress(string id) =>
        await _shippingAddressCollection.DeleteOneAsync(x => x.shipping_address_id == id);
}