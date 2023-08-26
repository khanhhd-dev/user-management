using DigitalPlatform.UserService.Database.Extensions;
using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Entity.Identity;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Database
{
    /// <summary>
    /// The class for DatabaseContext configuration
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci");
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                // Each User can have one entry in the Department join table
                b.HasOne(e => e.Department)
                    .WithMany(e => e.Users)
                    .HasForeignKey(ur => ur.DepartmentId);

                // Each User can have one entry in the JobTitle join table
                b.HasOne(e => e.JobTitle)
                    .WithMany(e => e.Users)
                    .HasForeignKey(ur => ur.JobTitleId);

            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

            });

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            //modelBuilder.ApplyGlobalFilters<IDtoEntityBase>(e => !e.IsDeleted);

            modelBuilder.CreateIndexIsDeleted();
            modelBuilder.Seed();
        }
    }
}
