using DigitalPlatform.UserService.DataAccess.UnitOfWork;
using DigitalPlatform.UserService.Domain._base.HandlerBase.CommandHandler;
using DigitalPlatform.UserService.Domain.Request.Commands.User;
using DigitalPlatform.UserService.Entity.Identity;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.UserService.Domain.Handler.Commands.User
{
    public class DeleteApplicationUserHandler : CommandDeleteHandlerBase<DeleteApplicationUserCommand, ApplicationUser>
    {
        public DeleteApplicationUserHandler(IUnitOfWork unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}