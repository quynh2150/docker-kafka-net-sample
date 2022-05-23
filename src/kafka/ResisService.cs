using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Newtonsoft.Json;

namespace KafkaNet
{
    [Route("api/[controller]")]
    public class RedisController : Controller
    {
        private readonly IDistributedCache _cache;

        public RedisController(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var cacheKey = "TheTime";
            var currentTime = DateTime.Now.ToString();

            // Trying to get data from the Redis cache
            var cachedTime = _cache.GetString(cacheKey);
            byte[] cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                // If the data is found in the cache, encode and deserialize cached data.
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
            }
            else
            {
                // Serializing the data
                string cachedDataString = "";
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                // Setting up the cache options
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                                                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                                                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                // Add the data into the cache
                await _cache.SetAsync(cacheKey, dataToCache, options);
            }
            
            if (string.IsNullOrEmpty(cachedTime))
            {
                // cachedTime = "Expired";
                // Cache expire in  5s
                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(5));

                // Nạp lại giá trị mới cho cache
                _cache.SetString(cacheKey, currentTime, options);
                cachedTime = _cache.GetString(cacheKey);
            }
            var result = $"Current Time : {currentTime} \nCached  Time : {cachedTime}";
            return result;
        }
    }
}