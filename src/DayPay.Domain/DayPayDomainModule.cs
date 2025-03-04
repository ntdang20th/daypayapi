using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace DayPay;

[DependsOn(
    typeof(DayPayDomainSharedModule),
    typeof(AbpDddDomainModule)
    )]
public class DayPayDomainModule : AbpModule
{
}
