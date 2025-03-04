using System;
using Volo.Abp.Domain.Entities;

namespace DayPay;

public class BaseEntity : Entity<Guid>
{
    public BaseEntity(Guid Id) => this.Id = Id;

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}