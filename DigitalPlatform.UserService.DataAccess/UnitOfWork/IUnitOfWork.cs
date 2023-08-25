using DigitalPlatform.UserService.DataAccess.Repository;
using DigitalPlatform.UserService.Database;
using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Entity._base;

namespace DigitalPlatform.UserService.DataAccess.UnitOfWork
{
    //
    // Summary:
    //     A abstract class provide functions/DbSets to work with database
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntityBase;

        #region Logging
        IRepository<Log> LogRepository { get; }
        #endregion Logging

        int SaveChanges();
        Task<int> SaveChangesAsync();
        DatabaseContext GetDatabaseContext();
    }
}
