using DigitalPlatform.UserService.DataAccess.Repository;
using DigitalPlatform.UserService.Database;
using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Entity.Identity;

namespace DigitalPlatform.UserService.DataAccess.UnitOfWork
{
    //
    // Summary:
    //     A abstract class provide functions/DbSets to work with database
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntityBase;

        IRepository<ApplicationUser> UserRepository { get; }
        IRepository<ApplicationRole> RoleRepository { get; }
        IRepository<JobTitle> JobTitleRepository { get; }
        IRepository<Department> DepartmentRepository { get; }

        #region Logging
        IRepository<Log> LogRepository { get; }
        #endregion Logging

        int SaveChanges();
        Task<int> SaveChangesAsync();
        DatabaseContext GetDatabaseContext();
    }
}
