using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class SalesOrdersService
{
    private readonly IMongoCollection<SalesOrder> _salesOrdersCollection;

    public SalesOrdersService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _salesOrdersCollection = mongoDatabase.GetCollection<SalesOrder>(
            csi5112BackEndDataBaseSettings.Value.SalesOrdersCollectionName);
    }

    public async Task<List<SalesOrder>> GetAllSalesOrders() =>
        await _salesOrdersCollection.Find(_ => true).ToListAsync();

    public async Task<List<SalesOrder>> GetSalesOrdersByID(string id) =>
        await _salesOrdersCollection.Find(x => x.order_id == id).ToListAsync();

    public async Task<List<SalesOrder>> GetSalesOrdersByMerchant(string id) =>
        await _salesOrdersCollection.Find(x => x.merchant_id == id).ToListAsync();

    public async Task<List<SalesOrder>> GetSalesOrdersByCustomer(string id) =>
        await _salesOrdersCollection.Find(x => x.customer_id == id).ToListAsync();

    public async Task CreateSalesOrder(SalesOrder newSalesOrder) =>
        await _salesOrdersCollection.InsertOneAsync(newSalesOrder);

    public async Task RemoveSalesOrder(string[] id) =>
        await _salesOrdersCollection.DeleteOneAsync(x => id.Contains(x.order_id));

    //  public async Task PlaceOrder(string[] id)
    // {
    //     List<CartItem> cartItems = await _cartItemsCollection.Find(x => x.customer_id == id).ToListAsync();
    //     List<CartProduct> cartProducts = new List<CartProduct>();
    //     foreach (CartItem cartItem in cartItems)
    //     {
    //         string product_id = cartItem.product_id;
    //         int quantity = cartItem.quantity;
    //         string itemId = cartItem.item_id;
    //         List<Product> products =await _ProductsService.GetProductsByID(product_id);
    //         CartProduct cartProduct = new CartProduct();
    //         if(products.Any()){
    //             cartProduct.product_id = products[0].product_id;
    //             cartProduct.category = products[0].category;
    //             cartProduct.description = products[0].description;
    //             cartProduct.image = products[0].image;
    //             cartProduct.image_type = products[0].image_type;
    //             cartProduct.price = products[0].price;
    //             cartProduct.owner = products[0].owner;
    //             cartProduct.owner_id = products[0].owner_id;
    //             cartProduct.date = products[0].date;
    //             cartProduct.manufacturer = products[0].manufacturer;
    //             cartProduct.name = products[0].name;
    //             cartProduct.quantity = quantity;
    //             cartProduct.item_id = itemId;
    //             cartProducts.Add(cartProduct);
    //         }
            
    //     }
    //     return cartProducts;
    // }
}