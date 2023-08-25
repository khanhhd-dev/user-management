using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler;
using DigitalPlatform.UserService.Domain._base.HandlerBase;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Domain.Request.Commands.Logging;
using DigitalPlatform.UserService.Share.Logging;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Domain.Handler.Commands.Logging
{
    public class ClearLogHandler :
        HandlerBase<ClearLogCommand, int>,
        ICommandHandlerBase<ClearLogCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private int _totalLogRecord;

        public ClearLogHandler(
            IUnitOfWork unitOfWork,
            ILogger logger) : base(unitOfWork, logger)
        {
            _unitOfWork = unitOfWork;
        }

        protected override async Task<IResultBase<int>> HandleAsync(ClearLogCommand command, RequestContextBase context)
        {
            if (!await TryBuildCommandAsync(command, context))
                return new ResultBase<int>(command.Messages.Contains(CommonMessages.DoNotHadPermission) ? 403 : 404, command.Messages.ToArray());

            await ExecuteAsync();

            return new ResultBase<int>(_totalLogRecord);
        }

        private async Task<int> ExecuteAsync()
        {
            return await UnitOfWork.SaveChangesAsync();
        }

        protected async Task<bool> TryBuildCommandAsync(ClearLogCommand command, RequestContextBase context)
        {
            _totalLogRecord = await _unitOfWork.LogRepository.GetQuery(true).CountAsync();
            await _unitOfWork.GetDatabaseContext().Database.ExecuteSqlRawAsync("TRUNCATE TABLE Logs");
            return true;
        }

        protected override async Task<bool> IsValidAsync(ClearLogCommand command)
        {
            await Task.CompletedTask;
            return true;
        }
    }
}
