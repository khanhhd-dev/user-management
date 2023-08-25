using DigitalPlatform.UserService.Database.Extensions;
using DigitalPlatform.UserService.Entity;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Database
{
    /// <summary>
    /// The class for DatabaseContext configuration
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci");
            base.OnModelCreating(modelBuilder);
            modelBuilder.CreateIndexIsDeleted();
            modelBuilder.CreateIndex();
        }
    }
}
