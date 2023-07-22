using System.Threading.Tasks;
using Data.Redis.Common.Interfaces;
using Newtonsoft.Json;

namespace Data.Redis.Common
{
    public class RedisCache<T> : IRedisCache<T> where T : new()
    {
        private IRedisConnector Connector { get; }

        public RedisCache(IRedisConnector connector)
        {
            Connector = connector;
        }


        public async Task<T> Get(string cacheKey)
        {
            var cache = Connector.Connection.GetDatabase();
            var objectString = cache.StringGet(cacheKey);
            if (objectString.IsNull)
                return default(T);
            return JsonConvert.DeserializeObject<T>(objectString);
        }

        public async Task Save(string cacheKey, T t)
        {
            var cache = Connector.Connection.GetDatabase();
            var objectString = JsonConvert.SerializeObject(t);
            cache.StringSet(cacheKey, objectString);
        }
    }
}
