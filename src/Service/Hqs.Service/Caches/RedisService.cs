
using CSRedis;
using Hqs.IService.Caches;
using Microsoft.Extensions.Configuration;

namespace Hqs.Service.Caches
{
    public class RedisService : ICacheService
    {
        public RedisService(IConfiguration configuration)
        {
            RedisHelper.Initialization(new CSRedisClient($"{configuration["ConnectionStrings:Redis"]},ssl=false"));
        }

        public void Set(string key, object value,int expireSeconds = -1)
        {
            RedisHelper.Set(key, value,expireSeconds);
        }

        public T Get<T>(string key)
        {
            return RedisHelper.Get<T>(key);
        }

        public void Remove(params string[] key)
        {
            RedisHelper.Del(key);
        }
    }
}
