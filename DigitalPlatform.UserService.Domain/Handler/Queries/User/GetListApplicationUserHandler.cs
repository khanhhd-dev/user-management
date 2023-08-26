using DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler;
using DigitalPlatform.UserService.Domain.Request.Queries.User;
using DigitalPlatform.UserService.Domain.Result.User;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Share;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.UserService.Domain.Handler.Queries.User
{
    public class GetListApplicationUserHandler : QueryListHandlerBase<GetListApplicationUserQuery, GetListApplicationUserResult>
    {
        public GetListApplicationUserHandler(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        protected override Task<IQueryable<GetListApplicationUserResult>> BuildQueryAsync(GetListApplicationUserQuery query, RequestContextBase context)
        {
            var q = UnitOfWork.UserRepository
                .GetQuery(c => c.UserType != CommonEnum.UserType.Customer
                               && c.Id != CommonConstants.SystemId);

            if (query.SearchText.IsValid())
            {
                q = q.Where(c => c.FullName.ToUpper().Contains(query.SearchText.ToUpper())
                                 || c.Email.ToUpper().Contains(query.SearchText.ToUpper())
                                 || c.UserName.ToUpper().Contains(query.SearchText.ToUpper())
                                 || c.JobTitle.Name!.ToUpper().Contains(query.SearchText.ToUpper())
                                 || c.Department.Name!.ToUpper().Contains(query.SearchText.ToUpper())
                                 || c.PhoneNumber.ToUpper().Contains(query.SearchText.ToUpper()));
            }

            if (query.IsActive.HasValue)
            {
                q = q.Where(c => c.IsActive == query.IsActive);
            }

            if (query.RoleId.HasValue)
            {
                q = q.Where(c => c.UserRoles.Any(a => a.RoleId == query.RoleId.Value));
            }

            if (query.DepartmentId.HasValue)
            {
                q = q.Where(c => c.DepartmentId == query.DepartmentId.Value);
            }

            if (query.UserType.HasValue)
            {
                q = q.Where(c => c.UserType == query.UserType);
            }

            var dataQuery = q.Select(x => new GetListApplicationUserResult
            {
                Id = x.Id,
                UserType = x.UserType,
                FullName = x.FullName,
                UserName = x.UserName,
                Email = x.Email,
                Gender = x.Gender.GetDescription(),
                PhoneNumber = x.PhoneNumber,
                IdNo = x.IdNo,
                RoleNames = string.Join(',', x.UserRoles.Select(t => t.Role.Name).ToList()).Replace(",", ", "),
                Department = x.Department.Name,
                Role = x.UserRoles.Select(c => c.Role.Name),
                IsActive = x.IsActive,
                InsertedAt = x.InsertedAt,
                UpdatedAt = x.UpdatedAt,
            });
            return Task.FromResult(dataQuery);
        }
    }
}
