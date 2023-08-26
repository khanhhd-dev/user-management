using DigitalPlatform.UserService.Api.Controllers._base;
using DigitalPlatform.UserService.Domain.Request.Commands.User;
using DigitalPlatform.UserService.Domain.Request.Queries.User;
using DigitalPlatform.UserService.Domain.Result.User;
using DigitalPlatform.UserService.Entity.Identity;
using MediatR;

namespace DigitalPlatform.UserService.Api.Controllers
{
    public class UserController : CrudControllerBase<ApplicationUser,
        GetItemApplicationUserQuery, GetItemApplicationUserResult,
        GetListApplicationUserQuery, GetListApplicationUserResult,
        CreateApplicationUserCommand,
        UpdateApplicationUserCommand,
        DeleteApplicationUserCommand>
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) : base(mediator)
        {

        }

    }
}