using Microsoft.EntityFrameworkCore;

namespace Database;

public class ImageDbContext : DbContext
{
    public DbSet<Image> Images { get; set; }
    public DbSet<Vector> Vectors { get; set; }
    public DbSet<SimilarImage> SimilarImages { get; set; }

    public IQueryable<SimilarImage> GetSimilarImages(string vector)
    {
        return SimilarImages.FromSqlRaw(
            "SELECT * FROM dbo.SimilarImages({0})", vector);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=localhost;Database=ReserveImageSearch;User=sa;Password=!MySecretTemplafyPassword1;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimilarImage>().HasNoKey();
        modelBuilder.Entity<Vector>().HasKey(v => new { v.ImageId, v.VectorPosition });
        base.OnModelCreating(modelBuilder);
    }
}