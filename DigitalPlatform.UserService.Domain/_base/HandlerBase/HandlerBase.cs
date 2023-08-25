using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share.Logging;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DigitalPlatform.UserService.Domain._base.HandlerBase
{
    public abstract class HandlerBase<TRequest, TResultType>
        where TRequest : IRequestBase<TResultType>
    {
        private readonly ILogger _logger;

        protected readonly IUnitOfWork UnitOfWork;

        protected HandlerBase(IUnitOfWork unitOfWork, ILogger logger)
        {
            UnitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IResultBase<TResultType>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //Gets request context
                var context = request.Context;

                PreProcessRequest(request);

                // Checks the request is valid or not
                if (await IsValidAllAsync(request))
                {
                    var handleResult = await HandleAsync(request, context);
                    await DoNext();
                    return handleResult;
                }
            }
            catch (Exception ex)
            {
                RollBack();
                await _logger.InsertLog(LogLevel.Error, request.Context.RequestId, request.ToJson(), ex.InnerException?.Message, ex.ToJson());
            }

            return new ResultBase<TResultType>
            {
                Success = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessages = request.Messages,
                TraceId = request.Context.RequestId
            };
        }

        protected abstract Task<IResultBase<TResultType>> HandleAsync(TRequest request, RequestContextBase context);

        protected async Task<bool> IsValidAllAsync(TRequest request)
        {
            if (request.IsValid())
            {
                return await IsValidAsync(request);
            }
            return false;
        }

        protected virtual async Task<bool> IsValidAsync(TRequest command)
        {
            await Task.CompletedTask;
            return true;
        }

        private void RollBack()
        {
            var context = UnitOfWork.GetDatabaseContext();
            var changedEntries = context.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        protected virtual async Task DoNext()
        {
            await Task.CompletedTask;
        }

        protected virtual void PreProcessRequest(TRequest request)
        {
            StringExtension.Trim(request);
        }
    }
}
