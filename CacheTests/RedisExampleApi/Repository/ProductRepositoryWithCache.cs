using RedisExample.Cache;
using RedisExampleApi.Models;

namespace RedisExampleApi.Repository
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisRepository _redisService;

        public ProductRepositoryWithCache(RedisRepository redisService,IProductRepository productRepository)
        {
            _redisService = redisService;
            _productRepository = productRepository;
        }
        public Task<Product> CreateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
