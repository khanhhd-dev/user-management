using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler
{
    public abstract class QueryHandlerBase<TQuery, TResultType> : HandlerBase<TQuery, TResultType>, IQueryHandlerBase<TQuery, TResultType>
        where TQuery : IQueryBase<TResultType>
    {
        protected QueryHandlerBase(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
