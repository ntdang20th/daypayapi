using Microsoft.AspNetCore.Mvc;
using System;

namespace DayPay.Controllers;

[Route("test")]
public sealed class TestController() : DayPayController
{
    [HttpGet]
    public ActionResult<int> GetRandom() => Ok(new Random().Next(1, 10));
}
