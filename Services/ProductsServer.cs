using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;

namespace CSI5112BackEndApi.Services;

public class ProductsService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductsService(
        IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _productsCollection = mongoDatabase.GetCollection<Product>(
            csi5112BackEndDataBaseSettings.Value.ProductsCollectionName);
    }

    public async Task<List<Product>> GetAllProducts() =>
        await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<Product?> GetProductsByID(string id) =>
        await _productsCollection.Find(x => x.product_id == id).FirstOrDefaultAsync();

    public async Task<List<Product>> GetProductsByMerchant(string id) =>
        await _productsCollection.Find(x => x.product_id == id).ToListAsync();

    public async Task CreateProduct(Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateProduct(string id, Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.product_id == id, updatedProduct);

    public async Task RemoveProduct(string id) =>
        await _productsCollection.DeleteOneAsync(x => x.product_id == id);
}