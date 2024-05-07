using CacheTests.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace CacheTests.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RedisWithModelProductsController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public RedisWithModelProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> SetRedisData()
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1)
            };

            Product2 p = new Product2
            {
                Id = 1,
                Name = "TestName",
                Price = 12
            };

            Product2 p2 = new Product2
            {
                Id = 2,
                Name = "TestName2",
                Price = 32
            };

            Product2 p3 = new Product2
            {
                Id = 3,
                Name = "TestName3",
                Price = 43
            };

            string jsonProduct = JsonConvert.SerializeObject(p);
            string jsonProduct2 = JsonConvert.SerializeObject(p2);
            await _distributedCache.SetStringAsync("Product:1", jsonProduct, distributedCacheEntryOptions);
            await _distributedCache.SetStringAsync("Product:2", jsonProduct2, distributedCacheEntryOptions);

            string jsonProduct3 = JsonConvert.SerializeObject(p3);
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct3);
            await _distributedCache.SetAsync("Product:3", byteProduct, distributedCacheEntryOptions);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRedisData()
        {
            string? jsonProducts = await _distributedCache.GetStringAsync("Product:1");
            Product2? p = JsonConvert.DeserializeObject<Product2>(jsonProducts);

            return Ok(p);
        }

        [HttpGet]
        public async Task<IActionResult> GetRedisByteData()
        {
            Byte[]? byteProduct = await _distributedCache.GetAsync("Product:3");

            string jsonProducts = Encoding.UTF8.GetString(byteProduct);
            Product2? p = JsonConvert.DeserializeObject<Product2>(jsonProducts);

            return Ok(p);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRedisData()
        {
            await _distributedCache.RemoveAsync("name");

            return Ok();
        }
    }
}
