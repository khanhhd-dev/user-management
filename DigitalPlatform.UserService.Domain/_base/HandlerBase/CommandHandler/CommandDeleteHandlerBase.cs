using DigitalPlatform.UserService.DataAccess.Repository;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.RequestBase.Command;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Share;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler
{
    public class CommandDeleteHandlerBase<TCommand, TEntity> : CommandHandlerBase<TCommand>
        where TCommand : ICommandDeleteBase
        where TEntity : class, IEntityBase
    {
        public CommandDeleteHandlerBase(IUnitOfWork unitOfWork, ILogger logger)
            : base(unitOfWork, logger)
        {
        }

        protected override async Task<bool> TryBuildCommandAsync(TCommand command, RequestContextBase context)
        {
            var repository = UnitOfWork.Repository<TEntity>();

            var deleteEntity = await GetDeleteEntityAsync(repository, command);

            if (deleteEntity == null)
            {
                command.Messages.Add(CommonMessages.TheItemDoesNotExist);
                return false;
            }

            repository.Delete(deleteEntity);
            return true;
        }

        protected virtual async Task<TEntity> GetDeleteEntityAsync(IRepository<TEntity> repository, TCommand command)
        {
            return await repository.GetByIdAsync(command.Id);
        }
    }
}
