using DayPay.EntityFrameworkCore.DbContext.Implements;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace DayPay.EntityFrameworkCore;

[DependsOn(
    typeof(DayPayDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class DayPayEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context) 
        => _ = context.Services.AddAbpDbContext<DayPayDbContext>(o => o.AddDefaultRepositories(includeAllEntities: true));
}
