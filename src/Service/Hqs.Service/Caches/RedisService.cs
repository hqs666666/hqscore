
using CSRedis;
using Hqs.IService.Caches;
using Microsoft.Extensions.Options;

namespace Hqs.Service.Caches
{
    public class RedisService 
        //: ICacheService
    {
        public RedisService(IOptions<AppSetting> options)
        {
            var setting = options.Value;
            RedisHelper.Initialization(new CSRedisClient($"{setting.ConnectionStrings.Redis},ssl=false"));
        }

        public void Set(string key, object value,int expireSeconds = -1)
        {
            RedisHelper.Set(key, value, expireSeconds);
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
