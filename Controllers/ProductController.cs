using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSI5112BackEndApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _ProductsService;

    public ProductsController(ProductsService ProductsService) =>
        _ProductsService = ProductsService;

    
    [HttpGet("all")]
    public async Task<ActionResult<List<Product>>> Get() {
        var product = await _ProductsService.GetAllProducts();
        return product;
    }

    [HttpGet("get_filter_option")]
    public async Task<FilterOption> GetFilterOption() {
        List<Product> res = await _ProductsService.GetAllProducts();
        HashSet<String> filter_category = new HashSet<string>();
        HashSet<String> filter_manufacturer = new HashSet<string>();
        foreach (Product p in res) {
            filter_category.Add(p.category);
            filter_manufacturer.Add(p.manufacturer);
        }
        String categories = "";
        String manufacturers = "";
        foreach (String s in filter_category) {
            categories += s;
            categories += "_";
        }
        foreach (String s in filter_manufacturer) {
            manufacturers += s;
            manufacturers += "_";
        }
        return new FilterOption("ascending", categories.Remove(categories.Length - 1, 1), manufacturers.Remove(manufacturers.Length - 1, 1));
    }


    [HttpGet]
    public async Task<List<Product>> Get([FromQuery]string product_id) =>
        await _ProductsService.GetProductsByID(product_id);

    [HttpGet("filter/owner")]
    public async Task<List<Product>> Get([FromQuery] string owner_id, [FromQuery] string input, [FromQuery] string priceSort, [FromQuery] string location, [FromQuery] string category)
    {
        String[] locations = location.Split('_');
        String[] categories = category.Split('_');
        
        var product = await _ProductsService.GetProductsByMerchant(owner_id, input == "#" ? "" : input, priceSort, locations, categories);

        return product;
    }

    [HttpGet("filter")]
    public async Task<List<Product>> Gets([FromQuery] string input, [FromQuery] string priceSort, [FromQuery] string location, [FromQuery] string category)
    {
        String[] locations = location.Split('_');
        String[] categories = category.Split('_');
        
        var product = await _ProductsService.GetProductsBySearch(input == "#" ? "" : input, priceSort, locations, categories);

        return product;
    }

    [HttpPost("create")]
    public async Task<List<Product>> Post(Product newProduct)
    {
        await _ProductsService.CreateProduct(newProduct);

        return await _ProductsService.GetAllProducts();
    }

    [HttpPut("update")]
    public async Task<List<Product>> Update([FromQuery]string id, Product updatedProduct)
    {
        var Product = await _ProductsService.GetProductsByID(id);

        updatedProduct.product_id = Product[0].product_id;

        await _ProductsService.UpdateProduct(id, updatedProduct);

        return await _ProductsService.GetAllProducts();
    }

    [HttpDelete("delete")]
    public async Task<List<Product>> Delete([FromBody] string id)
    {
        await _ProductsService.RemoveProduct(id.Split('_'));

        return await _ProductsService.GetAllProducts();
    }
}