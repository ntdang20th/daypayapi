using DayPay.Services;
using DayPay.Services.Implements;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace DayPay.Application.ElasticSearch;

[DependsOn(
    typeof(DayPayDomainModule),
    typeof(DayPayApplicationContractsModule)
)]
public class DayPayApplicationESModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<ESOptions>(configuration.GetSection("ElasticSearch"));

        _ = context.Services.AddSingleton(typeof(IESService<>), typeof(ESService<>));
    }
}
