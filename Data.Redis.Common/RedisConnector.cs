using System;
using Data.Redis.Common.Interfaces;
using StackExchange.Redis;

namespace Data.Redis.Common
{
    public class RedisConnector : IRedisConnector
    {
        private static Lazy<ConnectionMultiplexer> _connection;

        public RedisConnector(string host)
        {
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(host));
        }

        public ConnectionMultiplexer Connection => _connection.Value;
    }
}
