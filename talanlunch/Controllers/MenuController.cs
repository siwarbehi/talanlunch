using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Menus.Commands.AddMenu;
using TalanLunch.Application.Menus.Commands.DeleteMenu;
using TalanLunch.Application.Menus.Queries.GetAllMenus;
using TalanLunch.Application.Menus.Queries.GetMenuById;
using TalanLunch.Application.Menus.Commands.SetMenuOfTheDayCommand;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Menus.Commands.AddDishToMenu;
using Microsoft.AspNetCore.Authorization;

namespace Talanlunch.API.Controllers
{
    [Authorize]
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenuController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuAsync([FromBody] AddMenuCommand command)
        {
            try
            {
                var createdMenu = await _mediator.Send(command).ConfigureAwait(false);
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenu.MenuId }, createdMenu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu([FromRoute] int id)
        {
            var query = new GetMenuByIdQuery { MenuId = id };
            var menu = await _mediator.Send(query).ConfigureAwait(false);
            if (menu == null)
                return NotFound();

            return Ok(menu);
        }

        [HttpPut("{menuId}")]
        public async Task<ActionResult<AddDishToMenuCommandResult>> AddDishToMenu(int menuId, [FromBody] AddDishToMenuCommand command, CancellationToken cancellationToken)
        {
            command.MenuId = menuId;

            var result = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

            return !result.Succeeded ? NotFound(result.Error) : NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<Menu>> RemoveDishFromMenu([FromBody] RemoveDishFromMenuCommand command)
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenu(int id)
        {
            await _mediator.Send(new DeleteMenuCommand(id)).ConfigureAwait(false);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllMenusQueryResult>>> GetAllMenus([FromQuery] GetAllMenusQuery query)
        {
            var result = await _mediator.Send(query).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPatch("setMenuOfTheDay")]
        public async Task<IActionResult> SetMenuOfTheDay([FromBody] SetMenuOfTheDayCommand command)
        {
            var success = await _mediator.Send(command).ConfigureAwait(false);

            return success ? Ok("Menu of the day has been updated.") : BadRequest("Could not update menu of the day.");
        }
    }
}
