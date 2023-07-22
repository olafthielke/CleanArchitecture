using System;
using Data.Redis.Common.Interfaces;
using StackExchange.Redis;

namespace Data.Redis.Common
{
    // Start redis using command: redis-server.exe
    public class RedisConnector : IRedisConnector
    {
        private static Lazy<ConnectionMultiplexer> _connection;

        public RedisConnector(string host)
        {
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(host));
        }

        public RedisConnector(IRedisConfiguration config) 
            : this(config.Host)
        { }

        public ConnectionMultiplexer Connection => _connection.Value;
    }
}
