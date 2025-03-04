using DayPay.Requests;
using DayPay.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DayPay.Controllers;

[Route("api/categories")]
public sealed class CategoryController(ICategoryService categoryService) : DayPayController
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost]
    public async ValueTask<ActionResult<Guid>> AddCategory([FromBody] CategoryAddRequest request) => await _categoryService.AddCategory(request);
}
