using Microsoft.EntityFrameworkCore;
using WebApplication1.API.Domain;

namespace WebApplication1.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public AppDbContext() { }
    
    public DbSet<Studio> Studios { get; set; }
    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Studio>(e =>
        {
            e.HasKey(s => s.Id);
            e.HasMany(s => s.Games)
             .WithOne(g => g.Studio)
             .HasForeignKey(g => g.StudioId)
             .OnDelete(DeleteBehavior.Cascade);
            e.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(6) with time zone")
                .HasDefaultValueSql("now()");
            e.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(6) with time zone")
                .HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Game>(e =>
        {
            e.HasKey(g => g.Id);
            e.Property(g => g.StudioId)
             .IsRequired();
            e.Property(e => e.CreatedAt)
                .HasColumnType("timestamp(6) with time zone");
            e.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(6) with time zone");
        });

        base.OnModelCreating(modelBuilder);
    }
}