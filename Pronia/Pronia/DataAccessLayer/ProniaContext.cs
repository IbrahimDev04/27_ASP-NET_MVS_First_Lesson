using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DataAccessLayer
{
    public class ProniaContext : DbContext
    {
        public ProniaContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        ((BaseEntity)entry.Entity).CreatedTime = DateTime.Now;
                        ((BaseEntity)entry.Entity).isDeleted = false;
                        break;
                    case EntityState.Modified:
                        ((BaseEntity)entry.Entity).UpdateTime = DateTime.Now;
                        break;
                        // Handle other cases like EntityState.Modified if needed
                }
            }

                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ZEGA;Database=ProniaDb;Trusted_Connection=True;TrustServerCertificate=True;"); 
            base.OnConfiguring(optionsBuilder);
        }

    }
}
