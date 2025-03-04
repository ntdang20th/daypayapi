using DayPay.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using static DayPay.DayPayConsts.ConnectionStringName;

namespace DayPay.EntityFrameworkCore.DbContext;

[ConnectionStringName(Default)]
public interface IDayPayDbContext : IEfCoreDbContext
{
    public DbSet<Item> Items { get; }

    public DbSet<Category> Categories { get; }
}
