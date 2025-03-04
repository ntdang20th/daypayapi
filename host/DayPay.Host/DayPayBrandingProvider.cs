using DayPay.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace DayPay.Host;

[Dependency(ReplaceServices = true)]
public class DayPayBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<DayPayResource> _localizer;

    public DayPayBrandingProvider(IStringLocalizer<DayPayResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
