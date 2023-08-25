using DigitalPlatform.UserService.Domain._base.RequestBase.Command;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using MediatR;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler
{
    public interface ICommandHandlerBase<in TCommand, TResultType> : IRequestHandler<TCommand, IResultBase<TResultType>>
        where TCommand : ICommandBase<TResultType>
    {
    }
}