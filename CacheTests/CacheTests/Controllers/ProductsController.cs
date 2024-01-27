using CacheTests.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CacheTests.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        public ProductsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet(Name = "Products")]
        public async Task<IActionResult> GetAll()
        {
            if (!_memoryCache.TryGetValue("time", out string timeCache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                options.SlidingExpiration = TimeSpan.FromSeconds(10);

                _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);
            }

            List<Product> products = new List<Product>
            {
                new Product { Id = 1,Name="Test Product 1",
                    Time = _memoryCache.GetOrCreate<string>("time", a =>
                    {
                        return DateTime.Now.ToString();
                    }).ToString()},

                new Product { Id = 2,Name="Test Product 2",Time=timeCache},
                new Product { Id = 3,Name="Test Product 3",Time=_memoryCache.Get<string>("time").ToString()},
                new Product { Id = 4,Name="Test Product 4",Time=_memoryCache.Get<string>("time").ToString()},
            };

            return Ok(products);
        }
    }
}
