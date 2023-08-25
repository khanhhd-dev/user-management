using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.RequestBase.Command;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share;
using DigitalPlatform.UserService.Share.Logging;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler
{
    public abstract class CommandHandlerBase<TCommand> : HandlerBase<TCommand, int>, ICommandHandlerBase<TCommand, int>
        where TCommand : ICommandBase<int>
    {
        protected CommandHandlerBase(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }

        protected sealed override async Task<IResultBase<int>> HandleAsync(TCommand command, RequestContextBase context)
        {
            if (!await TryBuildCommandAsync(command, context))
                return new ResultBase<int>(command.Messages.Contains(CommonMessages.DoNotHadPermission) ? 403 : 404, command.Messages.ToArray());

            int result = await ExecuteAsync();

            await DoPostExecuteAsync(command, context);

            return new ResultBase<int>(result);
        }

        protected virtual async Task<int> ExecuteAsync()
        {
            try
            {
                return await UnitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null)
                    throw new Exception(dbEx.InnerException.ToString());
                throw new Exception(dbEx.ToString());
            }
        }

        protected abstract Task<bool> TryBuildCommandAsync(TCommand command, RequestContextBase context);

        protected virtual async Task DoPostExecuteAsync(TCommand command, RequestContextBase context)
        {
            await Task.CompletedTask;
        }
    }
}
