
namespace Hqs.IService.Caches
{
    public interface ICacheService
    {
        void Set(string key, object value, int expireSeconds = -1);
        T Get<T>(string key);
        void Remove(params string[] key);
    }
}
