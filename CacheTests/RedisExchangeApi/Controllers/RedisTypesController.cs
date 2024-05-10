using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RedisTypesController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        private readonly IDatabase _db2;
        private readonly IDatabase _db3;
        public string redisListText = "names";
        public string redisListText2 = "hashnames";

        public RedisTypesController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(0);
            _db2 = _redisService.GetDb(1);
            _db3 = _redisService.GetDb(2);
        }

        [HttpPost]
        public ActionResult SetStringValueDb()
        {
            _db.StringSet("name", "BurakTest");
            _db.StringSet("count", 3);

            return Ok();
        }

        [HttpGet]
        public ActionResult GetStringValueDb()
        {
            var value = _db.StringGet("name");
            var value2 = _db.StringGet("count");
            _db.StringIncrement("count", 1);

            if (value.HasValue)
            {
                return Ok(value.ToString() + " " + value2.ToString());
            }

            return Ok();
        }

        [HttpPost]
        public ActionResult SetRedisListValuesDb(string name)
        {
            _db2.ListRightPush(redisListText, name);

            return Ok();
        }

        [HttpGet]
        public ActionResult GetRedisListValuesDb()
        {
            List<string> redisList = new List<string>();

            if (_db2.KeyExists(redisListText))
            {
                _db2.ListRange(redisListText).ToList().ForEach(x =>
                {
                    redisList.Add(x);
                });
            }

            return Ok(redisList);
        }

        [HttpPost]
        public ActionResult RemoveRedisListItem(string name)
        {
            _db2.ListRemove(redisListText, name);

            return Ok();
        }

        [HttpPost]
        public ActionResult SetRedisSetValuesDb(string name) // unique values only
        {
            _db3.KeyExpire(redisListText2, DateTime.Now.AddMinutes(5));
            _db3.SetAdd(redisListText2, name);

            return Ok();
        }

        [HttpGet]
        public ActionResult GetRedisSetValuesDb()
        {
            HashSet<string> redisHashSetList = new HashSet<string>();

            if (_db3.KeyExists(redisListText2))
            {
                _db3.SetMembers(redisListText2).ToList().ForEach(x =>
                {
                    redisHashSetList.Add(x);
                });
            }

            return Ok(redisHashSetList);
        }

        [HttpPost]
        public ActionResult RemoveRedisSetItem(string name)
        {
            _db3.SetRemove(redisListText2, name);

            return Ok();
        }
    }
}
