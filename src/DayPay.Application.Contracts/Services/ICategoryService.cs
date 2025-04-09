using DayPay.Dtos.DayPayDto;
using DayPay.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DayPay.Services;

public interface ICategoryService : IApplicationService
{
    ValueTask<Guid> AddCategory(CategoryAddRequest request);

    ValueTask<IEnumerable<CategoryDto>> GetCategoriesAsync();
}
