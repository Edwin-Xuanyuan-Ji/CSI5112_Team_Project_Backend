using Microsoft.Extensions.Options;
using MongoDB.Driver;
using CSI5112BackEndApi.Models;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace CSI5112BackEndApi.Services;

public class ProductsService
{
    private readonly IMongoCollection<Product> _productsCollection;

    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
    private static readonly string awsAccessKeyId = "AKIAYBILP6CEHN3EMQGO";
    private static readonly string awsSecretAccessKey = "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";
    private static IAmazonS3 s3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, bucketRegion);

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

    public async Task UploadFileAsync(Stream FileStream, String keyName)
    {
        var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(FileStream, "csi5112pics", keyName);
    }

    public async Task<List<Product>> GetAllProducts() => 
        await _productsCollection.Find(_ => true).ToListAsync();

    public async Task<List<Product>> GetProductsByID(string id) =>
        await _productsCollection.Find(x => x.product_id == id).ToListAsync();

    public async Task<List<Product>> GetProductsByMerchant(string owner_id) =>
        await _productsCollection.Find(x => x.owner_id == owner_id).ToListAsync();
        
    public async Task<List<Product>> SortProductsByMerchant(string id, string input, string priceSort, string[] locations, string[] categories) {
        var sortPrice = priceSort == "ascending" ? Builders<Product>.Sort.Ascending("price") : Builders<Product>.Sort.Descending("price");
        return await _productsCollection.Find(x => locations.Contains(x.manufacturer) && categories.Contains(x.category) && x.name.ToLower().Contains(input.ToLower()) && x.owner_id == id).Sort(sortPrice).ToListAsync();
    }

    public async Task<List<Product>> SortProductsBySearch(string input, string priceSort, string[] locations, string[] categories) {
        var sortPrice = priceSort == "ascending" ? Builders<Product>.Sort.Ascending("price") : Builders<Product>.Sort.Descending("price");
        return await _productsCollection.Find(x => locations.Contains(x.manufacturer) && categories.Contains(x.category) && x.name.ToLower().Contains(input.ToLower())).Sort(sortPrice).ToListAsync();
    }

    public async Task<List<Product>> GetProductByCategory(string category, string id) =>
        await _productsCollection.Find(x => x.category == category && x.owner_id == id).ToListAsync();
        
    public async Task CreateProduct(Product newProduct) =>
        await _productsCollection.InsertOneAsync(newProduct);

    public async Task UpdateProduct(string id, Product updatedProduct) =>
        await _productsCollection.ReplaceOneAsync(x => x.product_id == id, updatedProduct);

    public async Task UpdateProducts(List<Product> products) {
        foreach (Product product in products)
        {
            await _productsCollection.ReplaceOneAsync(x => x.product_id == product.product_id, product);
        }
    }

    public async Task RemoveProduct(string[] id) =>
        await _productsCollection.DeleteManyAsync(x => id.Contains(x.product_id));
}