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

    public async Task<List<CartItem>> GetCartItemByID(string id) =>
        await _cartItemsCollection.Find(x => x.item_id == id).ToListAsync();

    public async Task<List<CartItem>> GetCartItemByCustomer(string id) =>
        await _cartItemsCollection.Find(x => x.customer_id == id).ToListAsync();

    public async Task CreateNewCartItem(CartItem newCartItem)

    {
        List<CartItem> findCartItems = await _cartItemsCollection.Find(x => (x.product_id == newCartItem.product_id) && (x.customer_id == newCartItem.customer_id)).ToListAsync();
        if (!findCartItems.Any())
        {
            await _cartItemsCollection.InsertOneAsync(newCartItem);
        }
        else
        {
            newCartItem.quantity = newCartItem.quantity + 1;
            newCartItem.item_id = findCartItems[0].item_id;
            await _cartItemsCollection.ReplaceOneAsync(x => (x.product_id == newCartItem.product_id) && (x.customer_id == newCartItem.customer_id), newCartItem);
        }

    }

    public async Task UpdateCartItem(string id, CartItem updatedCartItem) =>
        await _cartItemsCollection.ReplaceOneAsync(x => x.item_id == id, updatedCartItem);

    public async Task RemoveCartItem(string[] id) =>
        await _cartItemsCollection.DeleteOneAsync(x => id.Contains(x.item_id));



}