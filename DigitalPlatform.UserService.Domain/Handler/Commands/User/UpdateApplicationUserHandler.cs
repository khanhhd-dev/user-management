using DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler;
using DigitalPlatform.UserService.Domain.Request.Commands.User;
using DigitalPlatform.UserService.Entity.Identity;
using AutoMapper;
using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Share.Logging;
using Microsoft.AspNetCore.Identity;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;
using DigitalPlatform.UserService.Domain._base.RequestBase;

namespace DigitalPlatform.UserService.Domain.Handler.Commands.User
{
    public class UpdateApplicationUserHandler : CommandUpdateHandlerBase<UpdateApplicationUserCommand, ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private ApplicationUser ApplicationUser { get; set; }
        private bool _roleHasChange;
        private List<string> CurrentRoles { get; set; }
        private List<string> AddingRoles { get; set; }

        public UpdateApplicationUserHandler(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, 
            ILogger logger, 
            IMapper mapper) : base(unitOfWork, logger, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task<bool> IsValidAsync(UpdateApplicationUserCommand command)
        {
            ApplicationUser = await _userManager.FindByIdAsync(command.Payload.Id.ToString());
            if (ApplicationUser == null)
            {
                command.Messages.Add(CommonMessages.TheItemDoesNotExist);
                return false;
            }

            var oldRoles = await _userManager.GetRolesAsync(ApplicationUser);
            CurrentRoles = oldRoles.ToList();
            if (command.Payload.RoleIds is { Count: > 0 })
            {
                AddingRoles = await _roleManager.Roles
                    .Where(c => command.Payload.RoleIds.Contains(c.Id))
                    .Select(c => c.Name)
                    .ToListAsync();
            }
            else
            {
                AddingRoles = new List<string>();
            }
            _roleHasChange = CurrentRoles.Except(AddingRoles).Any() || AddingRoles.Except(CurrentRoles).Any();

            if (command.Payload.PhoneNumber.IsValid() && ApplicationUser.PhoneNumber != command.Payload.PhoneNumber)
            {
                if (await UnitOfWork.UserRepository.GetQuery().AnyAsync(c => c.PhoneNumber == command.Payload.PhoneNumber))
                {
                    command.Messages.Add(CommonMessages.UserMessage.PhoneNumberExisted);
                    return false;
                }
            }

            if (command.Payload.IdNo.IsValid() && ApplicationUser.IdNo != command.Payload.IdNo)
            {
                if (await UnitOfWork.UserRepository.GetQuery().AnyAsync(c => c.IdNo == command.Payload.IdNo))
                {
                    command.Messages.Add(CommonMessages.UserMessage.IdNoExisted);
                    return false;
                }
            }


            if (command.Payload.DepartmentId != null)
            {
                var department = await UnitOfWork.DepartmentRepository
                .GetQueryById(command.Payload.DepartmentId.Value)
                .FirstOrDefaultAsync();

                if (command.Payload.IsActive)
                {
                    if (!department.IsActive)
                    {
                        command.Messages.Add(CommonMessages.UserMessage.DepartmentOfUserInActive);
                        return false;
                    }
                }
            }

            return true;
        }

        protected override async Task<bool> TryBuildCommandAsync(UpdateApplicationUserCommand command, RequestContextBase context)
        {
            ApplicationUser.IsActive = command.Payload.IsActive;
            if (command.Payload.IsActive)
            {
                ApplicationUser.AccessFailedCount = 0;
                ApplicationUser.LockoutEnd = DateTime.UtcNow;
                ApplicationUser.LockoutEnabled = false;
            }
            ApplicationUser.FullName = command.Payload.FullName;
            ApplicationUser.Gender = command.Payload.Gender;
            ApplicationUser.JobTitleId = command.Payload.JobTitleId;
            ApplicationUser.DepartmentId = command.Payload.DepartmentId;
            ApplicationUser.UpdatedAt = DateTime.UtcNow;
            var updateUserResult = await _userManager.UpdateAsync(ApplicationUser);
            if (!updateUserResult.Succeeded)
            {
                command.Messages.Add(CommonMessages.UserMessage.UpdateUserFail);
                updateUserResult.Errors.Select(c => c.Description).ToList().ForEach(t =>
                {
                    command.Messages.Add(t);
                });
            }

            // update role
            if (_roleHasChange)
            {
                await _userManager.RemoveFromRolesAsync(ApplicationUser, CurrentRoles);
                await _userManager.AddToRolesAsync(ApplicationUser, AddingRoles);
            }
            return true;
        }
    }
}
