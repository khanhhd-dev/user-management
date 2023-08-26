using DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler;
using DigitalPlatform.UserService.Entity.Identity;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain.Request.Commands.User;
using DigitalPlatform.UserService.Share.Logging;
using DigitalPlatform.UserService.Domain._base.RequestBase;
using Microsoft.AspNetCore.Identity;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Domain.Handler.Commands.User
{
    public class CreateApplicationUserHandler : CommandCreateHandlerBase<CreateApplicationUserCommand, ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private ApplicationUser NewApplicationUser { get; set; }
        private string RandomGeneratePassword { get; set; }
        public CreateApplicationUserHandler(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger logger) : base(unitOfWork, logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            RandomGeneratePassword = StringExtension.GeneratePassword(3, 3, 3);
        }

        protected override async Task<bool> IsValidAsync(CreateApplicationUserCommand command)
        {
            if (await UnitOfWork.UserRepository.GetQuery().AnyAsync(c => c.Email == command.Payload.Email))
            {
                command.Messages.Add(CommonMessages.UserMessage.EmailExisted);
                return false;
            }

            if (command.Payload.PhoneNumber is not null
                && (await UnitOfWork.UserRepository.GetQuery().AnyAsync(c => c.PhoneNumber == command.Payload.PhoneNumber)))
            {
                command.Messages.Add(CommonMessages.UserMessage.PhoneNumberExisted);
                return false;
            }

            if (command.Payload.IdNo is not null
                && (await UnitOfWork.UserRepository.GetQuery().AnyAsync(c => c.IdNo == command.Payload.IdNo)))
            {
                command.Messages.Add(CommonMessages.UserMessage.IdNoExisted);
                return false;
            }

            return true;
        }

        protected override async Task<bool> TryBuildCommandAsync(CreateApplicationUserCommand command,
            RequestContextBase context)
        {

            NewApplicationUser = await PrepareApplicationUser(command.Payload);
            var createUserResult = await CreateUserAsync(NewApplicationUser, context.UserId, RandomGeneratePassword);
            if (!createUserResult.Succeeded)
            {
                var listErrorDesc = new List<string>();
                createUserResult.Errors.Select(c => c.Description).ToList().ForEach(t =>
                {
                    listErrorDesc.Add(t);
                });
                command.Messages.Add(CommonMessages.UserMessage.CreateUserFail);
                command.Messages.AddRange(listErrorDesc);
                return false;
            }

            var roles = new List<string>();
            if (command.Payload.RoleIds is { Count: > 0 })
            {
                roles = await _roleManager.Roles
                    .Where(c => c.IsActive && !c.IsDeleted && command.Payload.RoleIds.Contains(c.Id))
                    .Select(c => c.Name)
                    .ToListAsync();
                await AddUserToRolesAsync(NewApplicationUser, roles);
            }
            //send email
            PrepareNotificationEmailRequest(command, NewApplicationUser, roles);
            return true;
        }

        private async Task<IdentityResult> CreateUserAsync(ApplicationUser user, Guid userId,
            string password)
        {
            user.NormalizedUserName = user.UserName;
            user.NormalizedEmail = user.Email;
            user.IsActive = true;
            user.IsDeleted = false;
            user.InsertedById = userId;
            user.UpdatedById = userId;
            user.InsertedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.SecurityStamp = Guid.NewGuid().ToString();

            return await _userManager.CreateAsync(user, password);
        }
        private async Task AddUserToRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            await _userManager.AddToRolesAsync(user, roles);
        }

        private Task<ApplicationUser> PrepareApplicationUser(ApplicationUser request)
        {
            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                PhoneNumberConfirmed = true,
                Gender = request.Gender,
                JobTitleId = request.JobTitleId,
                DepartmentId = request.DepartmentId,
                IdNo = request.IdNo,
                UserType = request.UserType,
                IsActive = request.IsActive
            };

            return Task.FromResult(newUser);
        }

        private void PrepareNotificationEmailRequest(CreateApplicationUserCommand command,
            ApplicationUser newUser, IEnumerable<string> roles)
        {
            //TODO
        }
    }
}
