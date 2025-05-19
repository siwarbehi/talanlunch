using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Admin.Commands.ApproveCaterer;
using TalanLunch.Application.Admin.Commands.DeleteUser;
using TalanLunch.Application.Admin.Queries.GetPendingCaterersQuery;

namespace talanlunch.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // Récupérer tous les traiteurs en attente
        [HttpGet]
        public async Task<IActionResult> GetPendingCaterers([FromQuery] GetPendingCaterersQuery query)
        {
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                return NotFound("Aucun traiteur en attente.");
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> ApproveCaterer([FromBody] ApproveCatererCommand command)
        {
            var success = await _mediator.Send(command);

            if (!success)
            {
                return BadRequest("Impossible d'approuver le traiteur.");
            }

            return Ok("Le traiteur a été approuvé.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }
    }
}