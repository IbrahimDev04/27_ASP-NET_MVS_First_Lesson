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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ZEGA;Database=Pronia;Trusted_Connection=True;TrustServerCertificate=True;"); 
            base.OnConfiguring(optionsBuilder);
        }

    }
}
