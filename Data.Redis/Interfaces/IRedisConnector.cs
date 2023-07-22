using StackExchange.Redis;

namespace Data.Redis.Interfaces
{
    public interface IRedisConnector
    {
        ConnectionMultiplexer Connection { get; }
    }
}
