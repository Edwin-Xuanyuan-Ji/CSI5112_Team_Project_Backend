using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class CartItemsService
{
    private readonly IMongoCollection<CartItem> _cartItemsCollection;

    public CartItemsService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _cartItemsCollection = mongoDatabase.GetCollection<CartItem>(
            csi5112BackEndDataBaseSettings.Value.CartItemsCollectionName);
    }

    public async Task<List<CartItem>> GetAllCartItem() =>
        await _cartItemsCollection.Find(_ => true).ToListAsync();

    public async Task<CartItem?> GetCartItemByID(string id) =>
        await _cartItemsCollection.Find(x => x.item_id == id).FirstOrDefaultAsync();

    public async Task CreateNewCartItem(CartItem newCartItem) =>
        await _cartItemsCollection.InsertOneAsync(newCartItem);

    public async Task UpdateCartItem(string id, CartItem updatedCartItem) =>
        await _cartItemsCollection.ReplaceOneAsync(x => x.item_id == id, updatedCartItem);

    public async Task RemoveCartItem(string id) =>
        await _cartItemsCollection.DeleteOneAsync(x => x.item_id == id);
}