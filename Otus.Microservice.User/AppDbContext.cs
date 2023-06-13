using Microsoft.EntityFrameworkCore;

namespace Otus.Microservice.User;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(
        IConfiguration configuration,
        DbContextOptions<AppDbContext> options) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

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