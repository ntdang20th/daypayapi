using Volo.Abp.AspNetCore.Mvc;

namespace DayPay;

public abstract class DayPayController : AbpControllerBase
{
    protected DayPayController() => LocalizationResource = typeof(DayPayController);
}
