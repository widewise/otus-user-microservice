using Microsoft.EntityFrameworkCore;

namespace Otus.Microservice.User;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Models.User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Models.User>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        // Seed database with authors and books for demo
        new DbInitializer(builder).Seed();
    }
}