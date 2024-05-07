using CacheTests.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace CacheTests.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RedisProductsController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public RedisProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> SetRedisData()
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(100)
            };

            await _distributedCache.SetStringAsync("name", "Computer", distributedCacheEntryOptions);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRedisData()
        {
            string? productName = await _distributedCache.GetStringAsync("name");

            return Ok(productName);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRedisData()
        {
            await _distributedCache.RemoveAsync("name");

            return Ok();
        }
    }
}
