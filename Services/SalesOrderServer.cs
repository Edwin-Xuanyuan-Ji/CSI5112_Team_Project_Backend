using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class SalesOrdersService
{
    private readonly IMongoCollection<SalesOrder> _salesOrdersCollection;
    private readonly CartItemsService _cartItemsService;
    private readonly ProductsService  _productService;

    public SalesOrdersService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings, CartItemsService cartItemsService, ProductsService productsService)
    {
        _cartItemsService = cartItemsService;
        _productService = productsService;
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _salesOrdersCollection = mongoDatabase.GetCollection<SalesOrder>(
            csi5112BackEndDataBaseSettings.Value.SalesOrdersCollectionName);
    }

    public async Task<List<SalesOrder>> GetAllSalesOrders() =>
        await _salesOrdersCollection.Find(_ => true).ToListAsync();

    public async Task<List<SalesOrder>> SearchSalesOrdersByID(string id) => 
        await _salesOrdersCollection.Find(x => x.customer_id.ToLower().Contains(id.ToLower())).ToListAsync();

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

    public async Task PlaceOrder(List<PlaceOrdersFrontendRequire> placeOrdersFrontendRequires)
    {

        foreach (PlaceOrdersFrontendRequire placeOrdersFrontendRequire in placeOrdersFrontendRequires)
        {
            string customer_id = placeOrdersFrontendRequire.customer_id;
            DateTime date = placeOrdersFrontendRequire.date;
            string item_id = placeOrdersFrontendRequire.item_id;
            string shipping_address_id = placeOrdersFrontendRequire.shipping_address_id;
            string product_id = placeOrdersFrontendRequire.product_id;
            int? quantity = placeOrdersFrontendRequire.quantity;
            string status = "processing";
            List<Product> products = await _productService.GetProductsByID(product_id);
            if(products.Any()){
                string image = products[0].image;
                int? price = products[0].price;
                string merchant_id = products[0].owner_id;
                string name = products[0].name;
                SalesOrder salesOrder = new SalesOrder();
                salesOrder.customer_id = customer_id;
                salesOrder.date = date;
                salesOrder.image = image;
                salesOrder.merchant_id = merchant_id;
                salesOrder.name = name;
                salesOrder.price = price;
                salesOrder.product_id = product_id;
                salesOrder.quantity = quantity;
                salesOrder.shipping_address_id = shipping_address_id;
                salesOrder.status = status;
                // salesOrders.Add(salesOrder);
                await _salesOrdersCollection.InsertOneAsync(salesOrder);
            }
            string[] ids = new string[1];
            ids[0] = item_id;
            if((await _cartItemsService.GetCartItemByID(item_id)).Any()){
                await _cartItemsService.RemoveCartItem(ids);
            }
        }
         


    }
}