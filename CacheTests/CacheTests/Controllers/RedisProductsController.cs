using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CacheTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisProductsController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public RedisProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
    }
}
