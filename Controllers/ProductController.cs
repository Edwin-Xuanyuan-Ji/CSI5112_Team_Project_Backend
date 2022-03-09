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

    [HttpGet("filter/{owner_id}/{input}/{priceSort}/{location}/{category}")]
    public async Task<ActionResult<List<Product>>> Get(string owner_id, string input, string priceSort, string location, string category)
    {
        String[] locations = location.Split('_');
        String[] categories = category.Split('_');
        
        var product = await _ProductsService.GetProductsByMerchant(owner_id, input == "#" ? "" : input, priceSort, locations, categories);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpGet("filter/{input}/{priceSort}/{location}/{category}")]
    public async Task<ActionResult<List<Product>>> Get(string input, string priceSort, string location, string category)
    {
        String[] locations = location.Split('_');
        String[] categories = category.Split('_');
        
        var product = await _ProductsService.GetProductsBySearch(input == "#" ? "" : input, priceSort, locations, categories);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post(Product newProduct)
    {
        await _ProductsService.CreateProduct(newProduct);

        return CreatedAtAction(nameof(Get), new { id = newProduct.product_id }, newProduct);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(string id, Product updatedProduct)
    {
        var Product = await _ProductsService.GetProductsByID(id);

        if (Product is null)
        {
            return NotFound();
        }

        updatedProduct.product_id = Product.product_id;

        await _ProductsService.UpdateProduct(id, updatedProduct);

        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Product = await _ProductsService.GetProductsByID(id);

        if (Product is null)
        {
            return NotFound();
        }

        await _ProductsService.RemoveProduct(id);

        return NoContent();
    }
}