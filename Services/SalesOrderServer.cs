using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class SalesOrdersService
{
    private readonly IMongoCollection<SalesOrder> _salesOrdersCollection;
    private readonly CartItemsService _cartItemsService;
    private readonly ProductsService _productService;

    private readonly ShippingAddressService _shippingAddressService;

    public SalesOrdersService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings, CartItemsService cartItemsService, ProductsService productsService, ShippingAddressService shippingAddressService)
    {
        _cartItemsService = cartItemsService;
        _productService = productsService;
        _shippingAddressService = shippingAddressService;
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _salesOrdersCollection = mongoDatabase.GetCollection<SalesOrder>(
            csi5112BackEndDataBaseSettings.Value.SalesOrdersCollectionName);
    }

    public async Task<List<SalesOrder>> GetAllSalesOrders() =>
        await _salesOrdersCollection.Find(_ => true).ToListAsync();

    public async Task<List<SalesOrder>> SearchSalesOrdersByID(string customer_id, string merchant_id) =>
        await _salesOrdersCollection.Find(x => x.customer_id.ToLower().Contains(customer_id.ToLower()) && x.merchant_id.ToLower().Contains(merchant_id.ToLower())).ToListAsync();

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
            string customer_shipping_address_id = placeOrdersFrontendRequire.shipping_address_id;
            string product_id = placeOrdersFrontendRequire.product_id;
            int? quantity = placeOrdersFrontendRequire.quantity;
            string status = "processing";
            List<Product> products = await _productService.GetProductsByID(product_id);
            if (products.Any())
            {
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
                salesOrder.customer_shipping_address_id = customer_shipping_address_id;
                salesOrder.merchant_shipping_address_id = "";
                salesOrder.status = status;
                // salesOrders.Add(salesOrder);
                await _salesOrdersCollection.InsertOneAsync(salesOrder);
            }
            string[] ids = new string[1];
            ids[0] = item_id;
            if ((await _cartItemsService.GetCartItemByID(item_id)).Any())
            {
                await _cartItemsService.RemoveCartItem(ids);
            }
        }
    }

    public async Task<List<SalesOrderEcho>> SearchSalesOrderByUserId(string customer_id, string merchant_id, string role)
    {
        List<SalesOrder> salesOrders = new List<SalesOrder>();
        if ("Customer".Equals(role))
        {
            if ("#".Equals(merchant_id))
            {
                salesOrders = await GetSalesOrdersByCustomer(customer_id);
            }
            else
            {
                salesOrders = await SearchSalesOrdersByID(customer_id, merchant_id);

            }
        }
        else
        {
            if ("#".Equals(customer_id))
            {
                salesOrders = await GetSalesOrdersByMerchant(merchant_id);
            }
            else
            {
                salesOrders = await SearchSalesOrdersByID(customer_id, merchant_id);

            }
        }
        List<SalesOrderEcho> salesOrderEchoes = new List<SalesOrderEcho>();
        if (salesOrders.Any())
        {
            foreach (SalesOrder salesOrder in salesOrders)
            {
                SalesOrderEcho salesOrderEcho = new SalesOrderEcho();
                salesOrderEcho.salesOrder = salesOrder;
                string merchant_shipping_address_id = salesOrder.merchant_shipping_address_id;
                string customer_shipping_address_id = salesOrder.customer_shipping_address_id;
                if (!"".Equals(merchant_shipping_address_id))
                {
                    if ((await _shippingAddressService.GetShippingAddressByID(merchant_shipping_address_id)).Any())
                    {
                        ShippingAddress merchant_shippingAddress = (await _shippingAddressService.GetShippingAddressByID(merchant_shipping_address_id))[0];
                        salesOrderEcho.merchantAddress = merchant_shippingAddress;
                    }
                    else
                    {
                        salesOrderEcho.merchantAddress = new ShippingAddress();
                    }
                }
                else
                {
                    salesOrderEcho.merchantAddress = new ShippingAddress();
                }
                if (!"".Equals(customer_shipping_address_id))
                {
                    if ((await _shippingAddressService.GetShippingAddressByID(customer_shipping_address_id)).Any())
                    {
                        ShippingAddress customer_shippingAddress = (await _shippingAddressService.GetShippingAddressByID(customer_shipping_address_id))[0];
                        salesOrderEcho.customerAddress = customer_shippingAddress;
                    }
                    else
                    {
                        salesOrderEcho.customerAddress = new ShippingAddress();
                    }
                }
                else
                {
                    salesOrderEcho.customerAddress = new ShippingAddress();

                }

                salesOrderEchoes.Add(salesOrderEcho);


            }
            return salesOrderEchoes;
        }
        return new List<SalesOrderEcho>();

    }

    public async Task DeliverProduct(string id, string merchant_shipping_address_id)
    {
        if ((await GetSalesOrdersByID(id)).Any())
        {
            SalesOrder salesOrder = (await GetSalesOrdersByID(id))[0];
            salesOrder.merchant_shipping_address_id = merchant_shipping_address_id;
            salesOrder.status = "delivering";
            await _salesOrdersCollection.ReplaceOneAsync(x => x.order_id == id, salesOrder);
        }


    }
    public async Task RecieveProduct(string id)
    {
        if ((await GetSalesOrdersByID(id)).Any())
        {
            SalesOrder salesOrder = (await GetSalesOrdersByID(id))[0];
            salesOrder.status = "finish";
            await _salesOrdersCollection.ReplaceOneAsync(x => x.order_id == id, salesOrder);
        }

    }


}