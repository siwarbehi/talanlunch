using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Menus.Commands.AddMenu;
using TalanLunch.Application.Menus.Commands.DeleteMenu;
using TalanLunch.Application.Menus.Queries.GetAllMenus;
using TalanLunch.Application.Menus.Queries.GetMenuById;
using TalanLunch.Application.Menus.Commands.SetMenuOfTheDayCommand;

using TalanLunch.Domain.Entities;

namespace talanlunch.Controllers
{
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
                var createdMenu = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenu.MenuId }, createdMenu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET api/menu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu([FromRoute] int id)
        {
            var query = new GetMenuByIdQuery { MenuId = id };
            var menu = await _mediator.Send(query);
            if (menu == null)
                return NotFound();

            return Ok(menu);
        }


        /*      [HttpPost("{menuId}")]
              public async Task<ActionResult<AddDishToMenuResult>> AddDishToMenu(int menuId, [FromBody] AddDishToMenuDto dto, CancellationToken cancellationToken)
              {
                  if (!ModelState.IsValid)
                      return BadRequest(ModelState);

                  var result = await _mediator.Send(new AddDishToMenuCommand(menuId, dto));

                  if (result == null)
                      return NotFound("Menu introuvable ou plat invalide.");

                  if (result.DishAlreadyExists)
                      return BadRequest("Le plat existe déjà dans ce menu.");

                  return Ok(result);
              }*/


        /*   [HttpPost("{menuId}")]
           public async Task<ActionResult<AddDishToMenuResult>> AddDishToMenu(int menuId, [FromBody] AddDishToMenuCommand command)
           {
               if (!ModelState.IsValid)
                   return BadRequest(ModelState);

               command.MenuId = menuId; 

               var result = await _mediator.Send(command);

               if (result == null)
                   return NotFound("Menu introuvable ou plat invalide.");

               if (result.DishAlreadyExists)
                   return BadRequest(result);

               return Ok(result);
           }*/

        [HttpDelete]
        public async Task<ActionResult<Menu>> RemoveDishFromMenu([FromBody] RemoveDishFromMenuCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Supprimer un menu
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenu(int id)
        {
            await _mediator.Send(new DeleteMenuCommand(id));
            return NoContent();
        }


        // GET api/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllMenusQueryResult>>> GetAllMenus([FromQuery] GetAllMenusQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpPatch("setMenuOfTheDay")]
        public async Task<IActionResult> SetMenuOfTheDay([FromBody] SetMenuOfTheDayCommand command)
        {
            var success = await _mediator.Send(command);

            return success ? Ok("Menu of the day has been updated."): BadRequest("Could not update menu of the day.");
        }


    }
}