using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase.Command;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Share.Logging;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler
{
    public class CommandCreateHandlerBase<TCommand, TEntity> : CommandHandlerBase<TCommand>
        where TCommand : ICommandCreateBase<TEntity>
        where TEntity : class, IEntityBase
    {
        public CommandCreateHandlerBase(IUnitOfWork unitOfWork, ILogger logger)
            : base(unitOfWork, logger)
        {
        }

        protected override async Task<bool> TryBuildCommandAsync(TCommand command, RequestContextBase context)
        {
            var repository = UnitOfWork.Repository<TEntity>();
            var entity = await CreateEntityAsync(command);
            repository.Add(entity);
            return true;
        }

        protected virtual async Task<TEntity> CreateEntityAsync(TCommand command)
        {
            await Task.CompletedTask;
            return command.Payload;
        }

        protected override async Task<bool> IsValidAsync(TCommand command)
        {
            if (!await Exists(command)) return true;
            command.Messages.Add(CommonMessages.TheItemAlreadyExists);
            return false;
        }

        protected virtual async Task<bool> Exists(TCommand command)
        {
            return await UnitOfWork.Repository<TEntity>().GetQuery().AnyAsync(w => w.Id == command.Payload.Id);
        }
    }
}
