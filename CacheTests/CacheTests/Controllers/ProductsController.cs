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
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //{
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //}

            if (!_memoryCache.TryGetValue("time", out string timeCache))
            {
                _memoryCache.Set<string>("time", DateTime.Now.ToString());
            }

            List<Product> products = new List<Product>
            {
                new Product { Id = 1,Name="Test Product 1",
                    Time = _memoryCache.GetOrCreate<string>("time", a =>
                    {
                        return DateTime.Now.ToString();
                    }).ToString()},

                new Product { Id = 2,Name="Test Product 2",Time=_memoryCache.Get<string>("time").ToString()},
                new Product { Id = 3,Name="Test Product 3",Time=_memoryCache.Get<string>("time").ToString()},
                new Product { Id = 4,Name="Test Product 4",Time=_memoryCache.Get<string>("time").ToString()},
            };

            return Ok(products);
        }
    }
}
