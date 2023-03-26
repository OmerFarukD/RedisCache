using Microsoft.EntityFrameworkCore;
using RedisCache.WebAPI.Models;

namespace RedisCache.WebAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _appDbContext;

    public ProductRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _appDbContext.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _appDbContext.Products.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync() ?? throw new NullReferenceException();
    }

    public async Task<Product> CreateAsync(Product product)
    {
       await _appDbContext.Products.AddAsync(product);
       await _appDbContext.SaveChangesAsync();
       return product;
    }
}