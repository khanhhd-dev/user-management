using DigitalPlatform.UserService.Domain._base.RequestBase.Query;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler
{
    public interface IQuerySingleHandlerBase<in TQuery, TResultType> : IQueryHandlerBase<TQuery, TResultType>
        where TQuery : IQuerySingleBase<TResultType>
    {

    }
}
