using DigitalPlatform.UserService.Domain._base.RequestBase.Command;
using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Entity._base;
using MediatR;

namespace DigitalPlatform.UserService.Api.Controllers._base
{
    public abstract class CrudControllerBase<TEntity, TQueryItem, TGetItemViewModel, TQueryList, TGetListViewModel, TCreateCommand, TUpdateCommand, TDeleteCommand> : MyControllerBase
        where TEntity : class, IEntityBase
        where TCreateCommand : ICommandCreateBase<TEntity>
        where TUpdateCommand : ICommandUpdateBase<TEntity>
        where TDeleteCommand : class, ICommandDeleteBase
        where TQueryList : IQueryListBase<TGetListViewModel>
        where TQueryItem : IQueryByIdBase<TGetItemViewModel>
    {
        protected CrudControllerBase(IMediator mediator) : base(mediator)
        {

        }
    }
}
