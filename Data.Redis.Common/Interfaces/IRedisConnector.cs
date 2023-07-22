using StackExchange.Redis;

namespace Data.Redis.Common.Interfaces
{
    public interface IRedisConnector
    {
        ConnectionMultiplexer Connection { get; }
    }
}
