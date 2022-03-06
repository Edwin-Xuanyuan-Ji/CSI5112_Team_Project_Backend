using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class CustomersService
{
    private readonly IMongoCollection<Customer> _customersCollection;

    public CustomersService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _customersCollection = mongoDatabase.GetCollection<Customer>(
            csi5112BackEndDataBaseSettings.Value.CustomersCollectionName);
    }

    public async Task<List<Customer>> GetAsync() =>
        await _customersCollection.Find(_ => true).ToListAsync();

    public async Task<Customer?> GetAsync(string id) =>
        await _customersCollection.Find(x => x.customer_id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Customer newCustomer) =>
        await _customersCollection.InsertOneAsync(newCustomer);

    public async Task UpdateAsync(string id, Customer updatedCustomer) =>
        await _customersCollection.ReplaceOneAsync(x => x.customer_id == id, updatedCustomer);

    public async Task RemoveAsync(string id) =>
        await _customersCollection.DeleteOneAsync(x => x.customer_id == id);
}