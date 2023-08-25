using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.Helper;
using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share.Logging;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler
{
    public abstract class QueryListHandlerBase<TQuery, TResultType> : QueryHandlerBase<TQuery, PaginatedResult<TResultType>>, IQueryListHandlerBase<TQuery, TResultType>
        where TQuery : IQueryListBase<TResultType>
        where TResultType : BaseGetViewModel
    {
        protected QueryListHandlerBase(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }

        protected sealed override async Task<IResultBase<PaginatedResult<TResultType>>> HandleAsync(TQuery query, RequestContextBase context)
        {
            var queryable = await BuildQueryAsync(query, context);
            if (queryable == null)
            {
                return new ResultBase<PaginatedResult<TResultType>>(query.Messages.Contains(CommonMessages.DoNotHadPermission) ? 403 : 404, query.Messages.ToArray());
            }

            queryable = QueryableHelper.OrderThenPaging(query, ref queryable, out var totalCount);
            var results = await GetTranslatedResult(queryable);

            results = await DoPostExecuteAsync(results);
            return new ResultBase<PaginatedResult<TResultType>>(new PaginatedResult<TResultType>(
                query.PageNumber,
                query.PageSize,
                totalCount,
                results));
        }

        protected abstract Task<IQueryable<TResultType>> BuildQueryAsync(TQuery query, RequestContextBase context);

        protected virtual async Task<IList<TResultType>> DoPostExecuteAsync(IList<TResultType> results)
        {
            return await Task.FromResult(results);
        }

        protected virtual async Task<IList<TResultType>> GetTranslatedResult(IQueryable<TResultType> queryable)
        {
            return await queryable.ToListAsync();
        }
    }
}
