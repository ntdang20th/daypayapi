using DayPay.ConnectionFactories;
using DayPay.Dtos.RedisDto;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DayPay.Services.Implements;

public class RedisService<T> : IRedisService<T> where T : DayPayRedisDto
{
    private readonly ILogger<RedisService<T>> _logger;
    private readonly IRedisConnectionFactory _connectionFactory;
    private readonly ConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;

    public RedisService(ILogger<RedisService<T>> logger, IRedisConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
        _connectionMultiplexer = _connectionFactory.Connection();
        _connectionMultiplexer.UseElasticApm();
        _database = _connectionMultiplexer.GetDatabase();
    }
}
