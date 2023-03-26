using System.Text.Json;
using CacheImpl;
using RedisCache.WebAPI.Models;

namespace RedisCache.WebAPI.Repositories;

public class ProductRepositoryWithCache : IProductRepository
{
    private const string key = "productCaches";
    private readonly IProductRepository _productRepository;
    private readonly RedisService _redisService;

    public ProductRepositoryWithCache(IProductRepository productRepository, RedisService redisService)
    {
        _productRepository = productRepository;
        _redisService = redisService;
    }
    
    public async Task<List<Product>> GetAllAsync()
    {
        if (!await _redisService.GetDb(0).KeyExistsAsync(key))
        {
            return await LoadToCacheFromDbAsync();
        }

        List<Product> products = new List<Product>();

        var cachedProducts = await _redisService.GetDb(0).HashGetAllAsync(key);
        foreach (var cachedProduct in cachedProducts.ToList())
        {
            var product = JsonSerializer.Deserialize<Product>(cachedProduct.Value);
            products.Add(product);
        }

        return products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        if (await _redisService.GetDb(0).KeyExistsAsync(key))
        {
            var product = await _redisService.GetDb(0).HashGetAsync(key, id);
            return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
        }

        var products = await LoadToCacheFromDbAsync();
        return products.FirstOrDefault(x => x.Id == id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var newProduct = await _productRepository.CreateAsync(product);

        if (await _redisService.GetDb(0).KeyExistsAsync(key))
        {
            await _redisService.GetDb(0).HashSetAsync(key, product.Id, JsonSerializer.Serialize(newProduct));
        }

        return newProduct;
    }

    private async Task<List<Product>> LoadToCacheFromDbAsync()
    {
        var products = await _productRepository.GetAllAsync();
        
        products.ForEach(x =>
        {
            _redisService.GetDb(0).HashSet(key, x.Id, JsonSerializer.Serialize(x));
        });
        return products;
    }
}