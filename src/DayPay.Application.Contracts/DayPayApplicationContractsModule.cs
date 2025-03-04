using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace DayPay;

[DependsOn(
    typeof(DayPayDomainSharedModule),
    typeof(AbpDddApplicationContractsModule)
)]
public class DayPayApplicationContractsModule : AbpModule
{
}
