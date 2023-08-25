using AutoMapper;
using DigitalPlatform.UserService.DataAccess.Repository;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase.Command;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Share.Logging;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler
{
    //
    // Summary:
    //     An class provide basic functions for handling a command update data.
    public class CommandUpdateHandlerBase<TCommand, TEntity> : CommandHandlerBase<TCommand>
        where TCommand : ICommandUpdateBase<TEntity>
        where TEntity : class, IEntityBase
    {
        protected readonly IMapper Mapper;
        public CommandUpdateHandlerBase(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
            : base(unitOfWork, logger)
        {
            Mapper = mapper;
        }

        protected override async Task<bool> TryBuildCommandAsync(TCommand command, RequestContextBase context)
        {
            var repository = UnitOfWork.Repository<TEntity>();

            var oldEntity = await GetOldEntityAsync(repository, command);

            if (oldEntity == null)
            {
                command.Messages.Add(CommonMessages.TheItemDoesNotExist);
                return false;
            }

            await UpdateEntity(oldEntity, command);
            repository.Update(oldEntity);
            return true;
        }

        protected virtual async Task<TEntity> GetOldEntityAsync(IRepository<TEntity> repository, TCommand command)
        {
            return await repository.GetByIdAsync(command.Payload.Id);
        }

        protected virtual async Task UpdateEntity(TEntity oldEntity, TCommand command)
        {
            await Task.CompletedTask;
            Mapper.Map(command.Payload, oldEntity);
        }
    }
}
