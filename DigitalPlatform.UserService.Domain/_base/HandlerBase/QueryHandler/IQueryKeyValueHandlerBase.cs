using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Domain._base.ResultBase;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler
{
    public interface IQueryKeyValueHandlerBase<in TQuery, TResultType> : IQueryHandlerBase<TQuery, PaginatedResult<TResultType>>
        where TQuery : IQueryKeyValueBase<TResultType>
    {

    }
}
