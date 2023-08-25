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
    public abstract class QueryKeyValueHandlerBase<TQuery, TResultType> : QueryHandlerBase<TQuery, PaginatedResult<TResultType>>, IQueryListHandlerBase<TQuery, TResultType>
        where TQuery : IQueryKeyValueBase<TResultType>
        where TResultType : BaseGetKeyValueViewModel
    {
        protected QueryKeyValueHandlerBase(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }

        protected sealed override async Task<IResultBase<PaginatedResult<TResultType>>> HandleAsync(TQuery query, RequestContextBase context)
        {
            var queryable = await BuildQueryAsync(query, context);
            if (queryable == null)
            {
                return new ResultBase<PaginatedResult<TResultType>>(query.Messages.Contains(CommonMessages.DoNotHadPermission) ? 403 : 404, query.Messages.ToArray());
            }

            //Get total record
            var totalCount = queryable.Count();

            //Order
            queryable = query.OrderBy.IsValid() ? queryable.OrderBy(query.OrderBy, query.OrderByDirection) : queryable.OrderByDescending(c => c.Value);

            // paging
            if (query.IsPaged)
            {
                queryable = queryable.Skip(query.PageNumber * query.PageSize).Take(query.PageSize);
            }

            var results = await queryable.ToListAsync();
            return new ResultBase<PaginatedResult<TResultType>>(new PaginatedResult<TResultType>(
                query.PageNumber,
                query.PageSize,
                totalCount,
                results));
        }

        protected abstract Task<IQueryable<TResultType>> BuildQueryAsync(TQuery query, RequestContextBase context);
    }
}
