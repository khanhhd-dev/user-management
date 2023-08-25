using DigitalPlatform.UserService.Database;
using DigitalPlatform.UserService.Entity._base;

namespace DigitalPlatform.UserService.DataAccess.Repository
{
    public class Repository<T> : RepositoryBase<T, DatabaseContext> where T : class, IEntityBase
    {
        public Repository(DatabaseContext dataContext) : base(dataContext)
        {
        }
    }
}
