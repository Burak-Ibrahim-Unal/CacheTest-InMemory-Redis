using RedisExampleApi.Models;

namespace RedisExampleApi.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync(); // We return dtos here except entity...Its an example only...
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}
