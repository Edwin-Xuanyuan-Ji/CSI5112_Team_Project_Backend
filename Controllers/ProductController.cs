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

    [HttpGet]
    public async Task<List<Product>> Get() =>
        await _ProductsService.GetAllProducts();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<List<Product>>> Get(string owner)
    {
        var product = await _ProductsService.GetProductsByMerchant(owner);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Product newProduct)
    {
        await _ProductsService.CreateProduct(newProduct);

        return CreatedAtAction(nameof(Get), new { id = newProduct.product_id }, newProduct);
    }

    [HttpPut("{id:length(24)}")]
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

    [HttpDelete("{id:length(24)}")]
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