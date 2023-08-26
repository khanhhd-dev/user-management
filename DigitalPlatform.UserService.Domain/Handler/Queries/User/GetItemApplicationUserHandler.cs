using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.HandlerBase.QueryHandler;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Domain.Request.Queries.User;
using DigitalPlatform.UserService.Domain.Result.User;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.UserService.Domain.Handler.Queries.User
{
    public class GetItemApplicationUserHandler : QuerySingleHandlerBase<GetItemApplicationUserQuery, GetItemApplicationUserResult>
    {
        public List<string> UserRole { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid AgentId { get; set; }
        public GetItemApplicationUserHandler(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {

        }

        protected override async Task<IQueryable<GetItemApplicationUserResult>> BuildQueryAsync(GetItemApplicationUserQuery query, RequestContextBase context)
        {
            var q = UnitOfWork.UserRepository
                .GetQuery(c => c.Id == query.Id);

            var item = q.Select(x => new GetItemApplicationUserResult
            {
                Id = x.Id,
                UserType = x.UserType,
                FullName = x.FullName,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                IdNo = x.IdNo,
                Gender = x.Gender,
                IsActive = x.IsActive,
                DepartmentId = x.DepartmentId,
                Department = x.Department.Name!,
                JobTitle = x.JobTitle.Name!,
                JobTitleId = x.JobTitleId,
                Roles = x.UserRoles.Select(c => new BaseGetKeyValueViewModel
                {
                    Key = c.RoleId,
                    Value = c.Role.Name
                }),
                InsertedAt = x.InsertedAt,
                UpdatedAt = x.UpdatedAt
            });
            return await Task.FromResult(item);
        }
    }
}
