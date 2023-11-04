using Microsoft.EntityFrameworkCore;

namespace Featurize.Repositories.EntityFramework.Tests;

public class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options) : base(options)
    {
    }

    public DbSet<TestEntity> Entities { get;set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestEntity>()
            .HasKey(x => x.Id);
    }
}
