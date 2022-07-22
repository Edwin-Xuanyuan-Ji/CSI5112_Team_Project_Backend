using CSI5112BackEndApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CSI5112BackEndApi.Services;

public class CartItemsServer
{
    private readonly ProductsService _ProductsService;
    private readonly IMongoCollection<CartItem> _cartItemsCollection;


    public CartItemsServer(ProductsService productsService, IOptions<CSI5112BackEndDataBaseSettings> csi5112BackEndDataBaseSettings)
    {
        _ProductsService = productsService;
        var mongoClient = new MongoClient(
            csi5112BackEndDataBaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            csi5112BackEndDataBaseSettings.Value.DatabaseName);

        _cartItemsCollection = mongoDatabase.GetCollection<CartItem>(
            csi5112BackEndDataBaseSettings.Value.CartItemsCollectionName);
    }

    public async Task<List<CartProduct>> GetCustomerCartProducts(string id)
    {
        List<CartItem> cartItems = await _cartItemsCollection.Find(x => x.customer_id == id).ToListAsync();
        List<CartProduct> cartProducts = new List<CartProduct>();
        foreach (CartItem cartItem in cartItems)
        {
            string product_id = cartItem.product_id;
            int quantity = cartItem.quantity;
            string itemId = cartItem.item_id ?? "";
            List<Product> products =await _ProductsService.GetProductsByID(product_id);
            CartProduct cartProduct = new CartProduct();
            if(products.Any()){
                cartProduct.product_id = products[0].product_id;
                cartProduct.category = products[0].category;
                cartProduct.description = products[0].description;
                cartProduct.image = products[0].image;
                cartProduct.image_type = products[0].image_type;
                cartProduct.price = products[0].price;
                cartProduct.owner = products[0].owner;
                cartProduct.owner_id = products[0].owner_id;
                cartProduct.date = products[0].date;
                cartProduct.manufacturer = products[0].manufacturer;
                cartProduct.name = products[0].name;
                cartProduct.quantity = quantity;
                cartProduct.item_id = itemId;
                cartProducts.Add(cartProduct);
            }
            
        }
        return cartProducts;
    }

}