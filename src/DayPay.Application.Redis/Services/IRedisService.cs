using DayPay.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DayPay.Services;

public class IRedisService<T> : IApplicationService where T : DayPayRedisDto
{
}
