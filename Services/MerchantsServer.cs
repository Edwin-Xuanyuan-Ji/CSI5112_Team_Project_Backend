using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class MerchantsService
{
    private readonly IMongoCollection<Merchant> _merchantsCollection;

    public MerchantsService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _merchantsCollection = mongoDatabase.GetCollection<Merchant>(
            csi5112BackEndDataBaseSettings.Value.MerchantsCollectionName);
    }

    public async Task<List<Merchant>> GetAllMerchant() =>
        await _merchantsCollection.Find(_ => true).ToListAsync();

    public async Task<Merchant?> GetMerchantByID(string id) =>
        await _merchantsCollection.Find(x => x.merchant_id == id).FirstOrDefaultAsync();

    public async Task CreateNewMerchant(Merchant newMerchant) =>
        await _merchantsCollection.InsertOneAsync(newMerchant);

    public async Task UpdateMerchant(string id, Merchant updatedMerchant) =>
        await _merchantsCollection.ReplaceOneAsync(x => x.merchant_id == id, updatedMerchant);

    public async Task RemoveMerchant(string id) =>
        await _merchantsCollection.DeleteOneAsync(x => x.merchant_id == id);
}