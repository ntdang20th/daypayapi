namespace DayPay.Dtos.DayPayDto;

public sealed class CategoryDto : DomainDto
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
