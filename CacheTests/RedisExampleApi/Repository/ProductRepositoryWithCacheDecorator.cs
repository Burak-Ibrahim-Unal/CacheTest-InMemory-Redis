using RedisExample.Cache;
using RedisExampleApi.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApi.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisRepository _redisRepository;
        private readonly IDatabase _redisDb;
        private const string redisProductKey = "productsCache";

        public ProductRepositoryWithCacheDecorator(RedisRepository redisRepository, IProductRepository productRepository)
        {

            _redisRepository = redisRepository;
            _redisDb = _redisRepository.GetDb(2);
            _productRepository = productRepository;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            var addedProduct = await _productRepository.CreateAsync(product);
            if (await _redisDb.KeyExistsAsync(redisProductKey))
            {
                await _redisDb.HashSetAsync(redisProductKey, product.Id, JsonSerializer.Serialize(addedProduct));
            }

            return addedProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _redisDb.KeyExistsAsync(redisProductKey))
                return await LoadProductsFromCache();

            var productList = new List<Product>();
            var cachedProducts = await _redisDb.HashGetAllAsync(redisProductKey);

            foreach (var cachedProduct in cachedProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(cachedProduct.Value);
                productList.Add(product);
            }

            return productList;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (await _redisDb.KeyExistsAsync(redisProductKey))
            {
                var product = await _redisDb.HashGetAsync(redisProductKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadProductsFromCache();
            return products.FirstOrDefault(x => x.Id == id);
        }

        private async Task<List<Product>> LoadProductsFromCache()
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(product =>
            {
                _redisDb.HashSet(redisProductKey, product.Id, JsonSerializer.Serialize(product));
            });

            return products;
        }
    }
}
