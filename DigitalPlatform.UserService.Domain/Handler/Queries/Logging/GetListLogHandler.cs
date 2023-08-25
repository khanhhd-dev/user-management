using DigitalPlatform.UserService.DataAccess.Repository;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain.Request.Queries.Logging;
using DigitalPlatform.UserService.Domain.Result.Logging;
using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Share;
using DigitalPlatform.UserService.Share.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DigitalPlatform.UserService.Domain.Handler.Queries.Logging
{
    public class GetListLogHandler : QueryListHandlerBase<GetListLogQuery, GetListLogResult>
    {
        private readonly IRepository<Log> _logRepository;

        public GetListLogHandler(IUnitOfWork unitOfWork, ILogger logger)
            : base(unitOfWork, logger)
        {
            _logRepository = UnitOfWork.LogRepository;
        }

        protected override async Task<bool> IsValidAsync(GetListLogQuery query)
        {
            await Task.CompletedTask;
            return true;
        }

        protected override Task<IQueryable<GetListLogResult>> BuildQueryAsync(GetListLogQuery reqQuery, RequestContextBase context)
        {
            var queryLogs = _logRepository.GetQuery();

            if (reqQuery.LogLevelId != null)
                queryLogs = queryLogs.Where(c => c.LogLevelId == reqQuery.LogLevelId);

            if (reqQuery.FromDate.IsValid())
                queryLogs = queryLogs.Where(c => c.CreatedOnUtc >= reqQuery.FromDate);

            if (reqQuery.ToDate.IsValid())
                queryLogs = queryLogs.Where(c => c.CreatedOnUtc <= reqQuery.ToDate);

            if (reqQuery.ShortMessage.IsValid())
                queryLogs = queryLogs.Where(c => EF.Functions.Like(c.ShortMessage, $"%{reqQuery.ShortMessage.Trim()}%"));

            if (reqQuery.FullMessage.IsValid())
                queryLogs = queryLogs.Where(c => EF.Functions.Like(c.FullMessage, $"%{reqQuery.FullMessage.Trim()}%"));

            if (reqQuery.RequestId.IsValid())
                queryLogs = queryLogs.Where(c => c.RequestId == reqQuery.RequestId);

            if (reqQuery.RequestContent.IsValid())
                queryLogs = queryLogs.Where(c => EF.Functions.Like(c.RequestContent, $"%{reqQuery.RequestContent.Trim()}%"));

            if (reqQuery.ApiUrl.IsValid())
                queryLogs = queryLogs.Where(c => EF.Functions.Like(c.ApiUrl, $"%{reqQuery.ApiUrl.Trim()}%"));

            if (reqQuery.UserId.IsValid())
                queryLogs = queryLogs.Where(c => c.UserId == reqQuery.UserId);

            if (reqQuery.Username.IsValid())
                queryLogs = queryLogs.Where(c => EF.Functions.Like(c.Username, $"%{reqQuery.Username.Trim()}%"));

            var queryData = queryLogs.Select(x =>
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

            return Task.FromResult(queryData);
        }

        protected override async Task<IList<GetListLogResult>> DoPostExecuteAsync(IList<GetListLogResult> results)
        {
            foreach (var item in results)
            {
                item.LogLevelName = item.LogLevelId.ToString();
                item.FullMessage = JsonConvert.DeserializeObject((string)item.FullMessage);
                item.RequestContent = JsonConvert.DeserializeObject((string)item.RequestContent);
            }

            return await Task.FromResult(results);
        }
    }
}
