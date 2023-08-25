using DigitalPlatform.UserService.Api.Controllers._base;
using DigitalPlatform.UserService.Domain.Request.Commands.Logging;
using DigitalPlatform.UserService.Domain.Request.Queries.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalPlatform.UserService.Api.Controllers
{
    public class LogController : MyControllerBase
    {
        public LogController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var query = new GetItemLogQuery { Id = id };
            var item = await Mediator.Send(query);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var cmd = new DeleteLogCommand { Id = id };
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchAsync([FromBody] GetListLogQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAsync()
        {
            var cmd = new ClearLogCommand();
            var result = await Mediator.Send(cmd);
            return Ok(result);
        }
    }
}