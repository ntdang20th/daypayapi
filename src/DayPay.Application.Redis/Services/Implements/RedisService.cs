using DayPay.ConnectionFactories;
using DayPay.Dtos.RedisDto;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using static Newtonsoft.Json.JsonConvert;
using static System.Text.Encoding;
using static System.Threading.Tasks.Task;

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

    public async Task<IDictionary<string, T>> GetAll(string group)
    {
        try
        {
            if (group.IsNullOrWhiteSpace())
            {
                throw new BusinessException("Bad request!");
            }

            var rslts = new Dictionary<string, T>();
            var semSlim = new SemaphoreSlim(1);

            await WhenAll((await _database.HashGetAllAsync(group.ToLowerInvariant())).Where(e => e.Name.HasValue && e.Value.HasValue).Select(async x =>
            {
                var val = x.Value;

                if (val.HasValue)
                {
                    await semSlim.WaitAsync();

                    try
                    {
                        rslts.Add(x.Name.ToString(), DeserializeObject<T>(UTF8.GetString(val!)));
                    }
                    finally
                    {
                        _ = semSlim.Release();
                    }
                }
            }));

            return rslts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAll-RedisService-Exception: {Group}", group);

            throw;
        }
    }

    public async Task<bool> SetBulk(string group, IDictionary<string, T> fields)
    {
        try
        {
            if (group.IsNullOrWhiteSpace())
            {
                throw new BusinessException("Bad request!");
            }

            await _database.HashSetAsync(group.ToLowerInvariant(), [.. fields.Select(p => new HashEntry(p.Key.ToLowerInvariant(), SerializeObject(p.Value)))]);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RedisService-SetBulk-Exception: {Group} - {Fields}", group, SerializeObject(fields));

            throw;
        }
    }
}
