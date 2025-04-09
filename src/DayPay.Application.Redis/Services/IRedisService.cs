using DayPay.Dtos.RedisDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DayPay.Services;

public interface IRedisService<T> : IApplicationService where T : DayPayRedisDto
{
    Task<IDictionary<string, T>> GetAll(string group);

    Task<bool> SetBulk(string group, IDictionary<string, T> fields);
}
