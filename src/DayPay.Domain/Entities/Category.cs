using System;

namespace DayPay.Entities;

public sealed class Category(Guid Id) : BaseEntity(Id)
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}