using Microsoft.AspNetCore.Mvc;
using StockManagement.Application.Dto.Request;
using StockManagement.Application.Interfaces.Infrastructure;

namespace StockManagement.WebAPI.Controller;

[ApiController]
[Route("api/products")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]ProductCreateDto request)
    {
        var result = await productService.AddAsync(request);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody]ProductUpdateDto request)
    {
        var result = await productService.UpdateAsync(request);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await productService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery]ProductFilterDto request)
    {
        var result = await productService.GetListAsync(request);
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await productService.GetByIdAsync(id);
        return Ok(result);
    }
}