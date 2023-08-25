using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalPlatform.UserService.Api.Controllers._base
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MyControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;

        public MyControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}