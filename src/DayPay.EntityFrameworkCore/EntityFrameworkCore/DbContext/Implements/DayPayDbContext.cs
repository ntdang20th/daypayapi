using DayPay.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using static DayPay.DayPayConsts.ConnectionStringName;

namespace DayPay.EntityFrameworkCore.DbContext.Implements;

[ConnectionStringName(Default)]
public class DayPayDbContext(DbContextOptions<DayPayDbContext> options) : AbpDbContext<DayPayDbContext>(options), IDayPayDbContext
{
    public DbSet<Item> Items { get; set; }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
