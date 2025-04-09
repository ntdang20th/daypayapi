using DayPay.Dtos.DayPayDto;
using DayPay.Entities;
using DayPay.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using static DayPay.DayPayDomainErrorCodes;

namespace DayPay.Services;

public class CategoryService(
    ILogger<CategoryService> logger,
    IRepository<Category, Guid> categoryRepository
) : DayPayAppService, ICategoryService
{
    private readonly IRepository<Category, Guid> _categoryRepository = categoryRepository;
    private readonly ILogger<CategoryService> _logger = logger;

    public async ValueTask<Guid> AddCategory(CategoryAddRequest request)
    {
        try
        {
            //code validate
            var category = await _categoryRepository.FindAsync(x => x.Code == request.Code);

            if (category != null)
            {
                _logger.LogWarning("Create category: {Code} failed!", request.Code);
                throw new BusinessException(CODE_EXSISTED).WithData(nameof(request.Code), request.Code);
            }

            //add category
            var categoryId = Guid.NewGuid();
            _ = await _categoryRepository.InsertAsync(new Category(categoryId)
            {
                Code = request.Code,
                Name = request.Name,
                CreatedAt = request.CreatedAt,
                ModifiedAt = request.ModifiedAt
            });

            //success logging 
            _logger.LogInformation("Create category: {id} sucessfuly!", categoryId);

            return categoryId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CategoryService-AddCategory-Exception: {Request}", request.ToString());

            throw;
        }
    }

    public async ValueTask<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        try
        {
            return ObjectMapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(await _categoryRepository.GetListAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CategoryService-GetCategoriesAsync-Exception:");

            throw;
        }
    }
}
