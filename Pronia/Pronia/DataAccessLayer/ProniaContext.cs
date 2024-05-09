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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ZEGA;Database=ProniaDb;Trusted_Connection=True;TrustServerCertificate=True;"); 
            base.OnConfiguring(optionsBuilder);
        }

    }
}
