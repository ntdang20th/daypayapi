using DayPay.Dtos.DayPayDto;
using DayPay.Dtos.ESDto;
using DayPay.Dtos.RedisDto;
using DayPay.Requests;
using DayPay.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayPay.Controllers;

[Route("api/categories")]
public sealed class CategoryController(
    ICategoryService categoryService,
    IESService<ESCategoryDto> eSService,
    IRedisService<RedisCategoryDto> redisService
) : DayPayController
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IESService<ESCategoryDto> _eSService = eSService;
    private readonly IRedisService<RedisCategoryDto> _redisService = redisService;
    private readonly string _redisGroupName = "daypay:category";

    [HttpPost]
    public async Task<ActionResult<Guid>> AddCategory([FromBody] CategoryAddRequest request) => await _categoryService.AddCategory(request);

    [HttpGet]
    public async Task<ActionResult<CategoryDto>> GetAll() => Ok(await _categoryService.GetCategoriesAsync());

    [HttpPost("create-index-if-not-exists")]
    public async Task<IActionResult> CreateIndexIfNotExists()
    {
        await _eSService.CreateIndexIfNotExists();
        return Ok();
    }

    [HttpPost("sync-to-es")]
    public async Task<ActionResult<bool>> AddOrUpdateBulk() => await _eSService.AddOrUpdateBulk(ObjectMapper.Map<IEnumerable<CategoryDto>, IEnumerable<ESCategoryDto>>(await _categoryService.GetCategoriesAsync()));

    [HttpGet("from-es")]
    public async Task<ActionResult<ESCategoryDto>> GetAllFromES() => Ok(await _eSService.GetAll());

    [HttpPost("sync-to-redis")]
    public async Task<ActionResult<bool>> RedisSetBulk() => await _redisService.SetBulk(_redisGroupName,
        (await _categoryService.GetCategoriesAsync()).ToDictionary(x => x.Id.ToString(), ObjectMapper.Map<CategoryDto, RedisCategoryDto>));

    [HttpGet("from-redis")]
    public async Task<ActionResult<RedisCategoryDto>> GetAllFromRedis() => Ok(await _redisService.GetAll(_redisGroupName));
}
