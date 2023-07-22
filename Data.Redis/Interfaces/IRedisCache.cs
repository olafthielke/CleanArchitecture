using System.Threading.Tasks;

namespace Data.Redis.Interfaces
{
    public interface IRedisCache<T>
    {
        public Task<T> Get(string cacheKey);
        public Task Save(string cacheKey, T t);
    }
}
