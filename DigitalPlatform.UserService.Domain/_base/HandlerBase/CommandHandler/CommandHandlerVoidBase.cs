using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.RequestBase.Command;
using DigitalPlatform.UserService.Domain._base.ResultBase;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler
{

    public abstract class CommandHandlerVoidBase<TCommand> : ICommandHandlerBase<TCommand, IVoidResultBase>
        where TCommand : ICommandBase<IVoidResultBase>
    {
        protected CommandHandlerVoidBase(IUnitOfWork unitOfWork)
        {
        }

        public async Task<IResultBase<IVoidResultBase>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            await HandleAsync(request, new RequestContextBase());
            return new ResultBase<IVoidResultBase>();
        }

        protected abstract Task HandleAsync(TCommand command, RequestContextBase context);
    }
}
