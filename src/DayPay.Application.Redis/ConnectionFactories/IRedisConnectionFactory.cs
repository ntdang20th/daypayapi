using StackExchange.Redis;

namespace DayPay.ConnectionFactories;

public interface IRedisConnectionFactory
{
    ConnectionMultiplexer Connection();

    string ConnectionString();
}
