using DayPay.Localization;
using Volo.Abp.Application.Services;

namespace DayPay;

public abstract class DayPayAppService : ApplicationService
{
    protected DayPayAppService() => LocalizationResource = typeof(DayPayResource);
}
