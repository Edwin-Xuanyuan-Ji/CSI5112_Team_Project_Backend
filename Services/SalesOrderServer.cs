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
}