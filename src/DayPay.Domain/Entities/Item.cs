using System;

namespace DayPay.Entities;

public sealed class Item(Guid Id) : BaseEntity(Id)
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; } = 0;

    public Category Category { get; set; }
}