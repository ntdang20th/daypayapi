using DayPay.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace DayPay;

[DependsOn(
    typeof(DayPayApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class DayPayHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context) => ConfigureLocalization();

    private void ConfigureLocalization() => Configure<AbpLocalizationOptions>(o => o.Resources.Get<DayPayResource>().AddBaseTypes(typeof(AbpUiResource)));
}
