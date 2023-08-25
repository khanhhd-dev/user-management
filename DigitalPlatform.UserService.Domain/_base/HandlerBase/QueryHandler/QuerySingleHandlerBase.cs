using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share.Logging;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler
{
    public abstract class QuerySingleHandlerBase<TQuery, TResultType> : QueryHandlerBase<TQuery, TResultType>, IQuerySingleHandlerBase<TQuery, TResultType>
        where TQuery : IQuerySingleBase<TResultType>
        where TResultType : new()
    {
        protected QuerySingleHandlerBase(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        protected sealed override async Task<IResultBase<TResultType>> HandleAsync(TQuery query, RequestContextBase context)
        {
            var queryable = await BuildQueryAsync(query, context);

            if (queryable == null)
            {
                return new ResultBase<TResultType>(query.Messages.Contains(CommonMessages.DoNotHadPermission) ? 403 : 404, query.Messages.ToArray());
            }

            var result = await GetSingleResult(queryable);
            if (result == null)
            {
                return new ResultBase<TResultType>(404, CommonMessages.TheItemDoesNotExist);
            }
            return new ResultBase<TResultType>(result);
        }

        protected abstract Task<IQueryable<TResultType>> BuildQueryAsync(TQuery query, RequestContextBase context);

        protected virtual async Task<TResultType> GetSingleResult(IQueryable<TResultType> queryable)
        {
            return await queryable.FirstOrDefaultAsync();
        }
    }
}
