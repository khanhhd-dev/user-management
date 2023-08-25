using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain.Request.Queries.Logging;
using DigitalPlatform.UserService.Domain.Result.Logging;
using DigitalPlatform.UserService.Share;
using DigitalPlatform.UserService.Share.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DigitalPlatform.UserService.Domain.Handler.Queries.Logging
{
    public class GetItemLogHandler : QuerySingleHandlerBase<GetItemLogQuery, GetItemLogResult>
    {
        public GetItemLogHandler(
            IUnitOfWork unitOfWork,
            ILogger logger) : base(unitOfWork, logger)
        {

        }

        protected override async Task<bool> IsValidAsync(GetItemLogQuery command)
        {
            var logQuery = await UnitOfWork.LogRepository.GetByIdAsync(command.Id);
            if (logQuery == null)
            {
                command.Messages.Add(CommonMessages.TheItemDoesNotExist);
                return false;
            }

            return true;
        }

        protected override async Task<IQueryable<GetItemLogResult>> BuildQueryAsync(GetItemLogQuery query, RequestContextBase context)
        {
            var queryData = UnitOfWork.LogRepository.GetQueryById(query.Id).Select(x =>
                             new GetListLogResult
                             {
                                 Id = x.Id,
                                 LogLevelId = x.LogLevelId,
                                 ShortMessage = x.ShortMessage,
                                 FullMessage = x.FullMessage,
                                 RequestId = x.RequestId,
                                 RequestContent = x.RequestContent,
                                 ApiUrl = x.ApiUrl,
                                 UserId = x.UserId,
                                 Username = x.Username,
                                 CreatedOnUtc = x.CreatedOnUtc,
                                 InsertedAt = x.CreatedOnUtc,
                                 UpdatedAt = x.CreatedOnUtc,
                                 InsertedBy = x.Username,
                                 UpdatedBy = x.Username
                             });

            return await Task.FromResult(queryData);
        }

        protected override async Task<GetItemLogResult> GetSingleResult(IQueryable<GetItemLogResult> queryable)
        {
            var result = await queryable.FirstOrDefaultAsync();
            if (result != null)
            {
                result.LogLevelName = result.LogLevelId.ToString();
                result.FullMessage = JsonConvert.DeserializeObject((string)result.FullMessage);
                result.RequestContent = JsonConvert.DeserializeObject((string)result.RequestContent);
            }

            return result;
        }
    }
}
