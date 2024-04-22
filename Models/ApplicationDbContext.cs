using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CrafterCodes.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Torneos> Torneos { get; set; }
        public DbSet<Deporte> Deportes { get; set; }
    }
}
