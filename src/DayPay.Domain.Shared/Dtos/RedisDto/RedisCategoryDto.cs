namespace DayPay.Dtos.RedisDto;

public sealed class RedisCategoryDto : DayPayRedisDto
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
