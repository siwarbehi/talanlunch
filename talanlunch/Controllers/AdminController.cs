using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Admin.Commands.ApproveCaterer;
using TalanLunch.Application.Admin.Commands.DeleteUser;
using TalanLunch.Application.Admin.Queries.GetPendingCaterersQuery;

namespace Talanlunch.API.Controllers
{
    [Authorize]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingCaterers([FromQuery] GetPendingCaterersQuery query)
        {
            var result = await _mediator.Send(query).ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> ApproveCaterer([FromBody] ApproveCatererCommand command)
        {
            var success = await _mediator.Send(command).ConfigureAwait(false);

            return success ? Ok("Le traiteur a été approuvé.") : BadRequest("Impossible d'approuver le traiteur.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _mediator.Send(new DeleteUserCommand(id)).ConfigureAwait(false);
            return NoContent();
        }
    }
}
