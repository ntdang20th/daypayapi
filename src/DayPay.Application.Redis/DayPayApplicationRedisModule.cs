using DayPay.Services;
using DayPay.Services.Implements;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace DayPay.Application.Redis;

[DependsOn(
    typeof(DayPayDomainModule),
    typeof(DayPayApplicationContractsModule)
)]
public class DayPayApplicationRedisModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<RedisOptions>(x => x.RedisConnectionString = configuration["Redis:Configuration"]);

        _ = context.Services.AddSingleton(typeof(IRedisService<>), typeof(RedisService<>));
    }
}
