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

    public async Task<List<Customer>> GetAllCustomers() =>
        await _customersCollection.Find(_ => true).ToListAsync();

    public async Task<List<Customer>> GetCustomerByID(string id) =>
        await _customersCollection.Find(x => x.customer_id == id).ToListAsync();

    public async Task<List<Customer>> GetCustomerByUsername(string username) =>
        await _customersCollection.Find(x => x.username == username).ToListAsync();

    public async Task CreateNewCustomer(Customer newCustomer) =>
        await _customersCollection.InsertOneAsync(newCustomer);

    public async Task UpdateCustomer(string id, Customer updatedCustomer) =>
        await _customersCollection.ReplaceOneAsync(x => x.customer_id == id, updatedCustomer);

    public async Task RemoveCustomer(string[] ids) =>
        await _customersCollection.DeleteManyAsync(x => ids.Contains(x.customer_id));
}