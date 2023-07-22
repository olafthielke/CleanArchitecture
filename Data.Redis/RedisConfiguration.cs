using Data.Redis.Interfaces;

namespace Data.Redis
{
    public class RedisConfiguration : IRedisConfiguration
    {
        // Imagine that Host is being read from a config file.
        public string Host => "localhost";
    }
}
