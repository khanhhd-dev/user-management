using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using MediatR;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler
{
    public interface IQueryHandlerBase<in TQuery, TResultType> : IRequestHandler<TQuery, IResultBase<TResultType>>
        where TQuery : IQueryBase<TResultType>
    {
    }
}
