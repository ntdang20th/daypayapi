using AutoMapper;
using DayPay.Dtos.DayPayDto;
using DayPay.Dtos.ESDto;
using DayPay.Dtos.RedisDto;
using DayPay.Entities;

namespace DayPay.Mappers;

public sealed class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        _ = CreateMap<Category, CategoryDto>();

        _ = CreateMap<CategoryDto, ESCategoryDto>();

        _ = CreateMap<CategoryDto, RedisCategoryDto>();
    }
}
