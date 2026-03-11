using Microsoft.EntityFrameworkCore;
//using Ulak.Domain.Entities;

namespace Ulak.Persistence.Context
{
    public class UlakDbContext : DbContext
    {
        public UlakDbContext(DbContextOptions<UlakDbContext> options) : base(options)
        {
        }

        // Örnek DbSet
        //public DbSet<User> Users { get; set; }  // ileride entity ekle
    }
}