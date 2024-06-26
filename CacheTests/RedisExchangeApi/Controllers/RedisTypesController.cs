﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Services;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        private readonly IDatabase _db4;
        private readonly IDatabase _db5;
        public string redisListText = "names";
        public string redisListText2 = "hashnames";
        public string redisListText3 = "hashsortednames";
        public string hashKey { get; set; } = "redishashnames";

        public RedisTypesController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(0);
            _db2 = _redisService.GetDb(1);
            _db3 = _redisService.GetDb(2);
            _db4 = _redisService.GetDb(3);
            _db5 = _redisService.GetDb(4);
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

        //List Start
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
        //List End

        //Set Start
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
        //Set End

        //Sorted Set Start
        [HttpPost]
        public ActionResult SetRedisSortedSetValuesDb(string name, int sortbyScore) // sorted unique values only
        {
            _db4.KeyExpire(redisListText3, DateTime.Now.AddMinutes(5));

            _db4.SortedSetAdd(redisListText3, name, sortbyScore);

            return Ok();
        }

        [HttpGet]
        public ActionResult GetRedisSortedSetValuesDb()
        {
            HashSet<string> redisSortedHashSetList = new HashSet<string>();

            if (_db4.KeyExists(redisListText3))
            {
                //1.Way - Rank and values
                //_db4.SortedSetScan(redisListText3).ToList().ForEach(x =>
                //{
                //    redisSortedHashSetList.Add(x.ToString());
                //});

                //2. Way - Value only
                _db4.SortedSetRangeByRank(redisListText3, order: Order.Descending).ToList().ForEach(x =>
                {
                    redisSortedHashSetList.Add(x.ToString());
                });
            }

            return Ok(redisSortedHashSetList);
        }

        [HttpPost]
        public ActionResult RemoveRedisSortedSetItem(string name)
        {
            _db4.SortedSetRemove(redisListText3, name);

            return Ok();
        }
        //Sorted Set End

        //Redis Hash Start
        [HttpPost]
        public ActionResult SetHashValuesDb(string key, string name)
        {
            _db5.KeyExpire(hashKey, DateTime.Now.AddMinutes(5));

            _db5.HashSet(hashKey, key, name);

            return Ok();
        }

        [HttpGet]
        public ActionResult GetHashValuesDb()
        {
            Dictionary<string, string> redisHashList = new Dictionary<string, string>();

            if (_db5.KeyExists(hashKey))
            {
                _db5.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    redisHashList.Add(x.Name, x.Value);
                });
            }

            return Ok(redisHashList);
        }

        [HttpPost]
        public ActionResult RemoveHashItem(string key)
        {
            _db5.HashDelete(hashKey, key);

            return Ok();
        }
        //Redis Hash End
    }
}
