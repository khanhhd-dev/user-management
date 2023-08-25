using DigitalPlatform.UserService.DataAccess.Repository;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain.Request.Commands.Logging;
using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Share;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.EVoucherEventService.Domain.Handler.Commands.Logging
{
    public class DeleteLogHandler : CommandDeleteHandlerBase<DeleteLogCommand, Log>
    {
        private readonly IRepository<Log> _logRepository;
        private Log _deletedLog;

        public DeleteLogHandler(IUnitOfWork unitOfWork, ILogger logger)
            : base(unitOfWork, logger)
        {
            _logRepository = UnitOfWork.LogRepository;
        }

        protected override async Task<bool> TryBuildCommandAsync(DeleteLogCommand command, RequestContextBase context)
        {
            _logRepository.Delete(_deletedLog);

            await Task.CompletedTask;
            return true;
        }

        protected override async Task<bool> IsValidAsync(DeleteLogCommand command)
        {
            _deletedLog = await GetDeleteEntityAsync(_logRepository, command);
            if (_deletedLog == null)
            {
                command.Messages.Add(CommonMessages.TheItemDoesNotExist);
                return false;
            }

            return true;
        }
    }
}
