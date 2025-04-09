using DayPay.Application.ElasticSearch;
using DayPay.Application.Redis;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace DayPay;

[DependsOn(
    typeof(DayPayDomainModule),
    typeof(DayPayApplicationContractsModule),
    typeof(DayPayApplicationESModule),
    typeof(DayPayApplicationRedisModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreSignalRModule)
)]

public class DayPayApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
        => Configure<AbpAutoMapperOptions>(options => options.AddMaps<DayPayApplicationModule>());
}
