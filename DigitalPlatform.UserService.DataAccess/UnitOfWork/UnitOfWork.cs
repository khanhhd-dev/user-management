using System.Security.Claims;
using DigitalPlatform.UserService.DataAccess.Repository;
using DigitalPlatform.UserService.Database;
using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Entity.Identity;
using DigitalPlatform.UserService.Share;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;

namespace DigitalPlatform.UserService.DataAccess.UnitOfWork
{
    //
    // Summary:
    //     A abstract class provide functions/DbSets to work with database
    public class UnitOfWork : IUnitOfWork
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected DatabaseContext DataContext { get; set; }
        protected Dictionary<string, object> Repositories { get; set; }
        public UnitOfWork(DatabaseContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            DataContext = dataContext;
            HttpContextAccessor = httpContextAccessor;
            Repositories = new Dictionary<string, object>();
        }

        private IRepository<ApplicationUser> _userRepository;
        public IRepository<ApplicationUser> UserRepository => _userRepository ??= new Repository<ApplicationUser>(DataContext);

        private IRepository<ApplicationRole> _roleRepository;
        public IRepository<ApplicationRole> RoleRepository => _roleRepository ??= new Repository<ApplicationRole>(DataContext);

        private IRepository<JobTitle> _jobTitleRepository;
        public IRepository<JobTitle> JobTitleRepository => _jobTitleRepository ??= new Repository<JobTitle>(DataContext);

        private IRepository<Department> _departmentRepository;
        public IRepository<Department> DepartmentRepository => _departmentRepository ??= new Repository<Department>(DataContext);

        #region Logging
        private IRepository<Log> _logRepository;
        public IRepository<Log> LogRepository => _logRepository ??= new Repository<Log>(DataContext);
        #endregion Logging

        public virtual IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntityBase
        {
            string key = typeof(TEntity).ToString();
            if (!Repositories.TryGetValue(key, out var repository))
            {
                repository = new Repository<TEntity>(DataContext);
                Repositories[key] = repository;
            }
            return (IRepository<TEntity>)repository;
        }

        public DatabaseContext GetDatabaseContext()
        {
            return DataContext;
        }

        public int SaveChanges()
        {
            try
            {
                SaveChangesDetail();
                return DataContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message, new List<IUpdateEntry>());
            }
        }

        public Task<int> SaveChangesAsync()
        {
            try
            {
                SaveChangesDetail();
                return DataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message, new List<IUpdateEntry>());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
                DataContext?.Dispose();
        }

        private void SaveChangesDetail()
        {
            var userClaim = HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            var userName = HttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
            Guid userId;
            if (userClaim == null)
            {
                userId = CommonConstants.SystemId;
            }
            else
            {
                var userIdString = userClaim.Value;
                if (!string.IsNullOrEmpty(userIdString))
                {
                    Guid.TryParse(userIdString, out userId);
                }
                else
                {
                    userId = CommonConstants.SystemId;
                }
            }

            var entries = DataContext.ChangeTracker.Entries();
            foreach (var e in entries)
            {
                if (e.Entity is IEntityBase entity)
                {
                    switch (e.State)
                    {
                        case EntityState.Added:
                            entity.InsertedById = userId;
                            entity.InsertedBy = userName;
                            entity.InsertedAt = DateTime.UtcNow;
                            entity.UpdatedById = userId;
                            entity.UpdatedBy = userName;
                            entity.UpdatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedById = userId;
                            entity.UpdatedBy = userName;
                            entity.UpdatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        case EntityState.Deleted:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
