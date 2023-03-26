using Microsoft.AspNetCore.Mvc;
using RedisCache.WebAPI.Models;
using RedisCache.WebAPI.Repositories;

namespace RedisCache.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
     private readonly IProductRepository _productRepository;

     public ProductsController(IProductRepository productRepository)
     {
          _productRepository = productRepository;
     }

     [HttpGet]
     public async Task<IActionResult> GetAll()
     {
          var data = await _productRepository.GetAllAsync();
          return Ok(data);
     }

     [HttpGet("{id}")]
     public async Task<IActionResult> GetByIdAsync(int id)
     {
          return Ok(await _productRepository.GetByIdAsync(id));
     }

     [HttpPost]
     public async Task<IActionResult> CreateAsync([FromBody] Product product)
     {
          return Ok(await _productRepository.CreateAsync(product));
     }
}