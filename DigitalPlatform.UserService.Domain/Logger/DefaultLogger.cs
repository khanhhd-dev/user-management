using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Share.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DigitalPlatform.UserService.Domain.Logger
{
    public class DefaultLogger : ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public DefaultLogger(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task InsertLog(LogLevel logLevel, Guid? requestId, string requestContent, string shortMessage, string fullMessage = "")
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Request == null)
                return;

            var context = _httpContextAccessor.HttpContext;
            var log = new Log
            {
                LogLevelId = logLevel,
                RequestId = requestId,
                RequestContent = requestContent,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                ApiUrl = context.Request.Method + ": " + context.Request.Path,
                Username = context.User?.FindFirst(ClaimTypes.Name)?.Value,
                CreatedOnUtc = DateTime.UtcNow
            };

            var userClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim != null && !string.IsNullOrEmpty(userClaim.Value))
            {
                Guid.TryParse(userClaim.Value, out var userId);
                log.UserId = userId;
            }

            await SaveToDbAsync(log);
        }

        private async Task SaveToDbAsync(Log log)
        {
            try
            {
                _unitOfWork.LogRepository.Add(log);
                await _unitOfWork.GetDatabaseContext().SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                if (dbEx.InnerException != null)
                    throw new Exception(dbEx.InnerException.ToString());
                throw new Exception(dbEx.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
