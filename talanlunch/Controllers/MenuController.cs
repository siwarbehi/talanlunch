using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Commands.Menu;

using TalanLunch.Application.Dtos.Menu;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Queries.Menu;
using TalanLunch.Domain.Entities;

namespace talanlunch.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMediator _mediator;

        public MenuController(IMenuService menuService , IMediator mediator)
        {
            _menuService = menuService;
            _mediator = mediator;
        }

        /*// Creation d un menu 
        [HttpPost]
        public async Task<IActionResult> AddMenuAsync([FromBody] MenuDto menuDto)
        {
            if (menuDto == null)
                return BadRequest("Le menu ne peut pas être null.");

            try
            {
                var createdMenu = await _menuService.AddMenuAsync(menuDto);
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenu.MenuId }, createdMenu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Une erreur inattendue est survenue.");
            }
        }*/

        [HttpPost]
        public async Task<IActionResult> AddMenuAsync([FromBody] MenuDto menuDto)
        {
            try
            {
                var createdMenu = await _mediator.Send(new AddMenuCommand(menuDto));
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenu.MenuId }, createdMenu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*[HttpPost("{menuId}")]
        public async Task<ActionResult<AddDishToMenuResultDto>> AddDishToMenu(int menuId, [FromBody] AddDishToMenuDto dto)
        {
            var result = await _menuService.AddDishToMenuAsync(menuId, dto);

            if (result == null)
                return NotFound("Menu introuvable ou plat invalide.");

            var responseDto = new AddDishToMenuResultDto
            {
                DishAlreadyExists = result.DishAlreadyExists
            };

            if (result.DishAlreadyExists)
                return BadRequest(responseDto);

            return Ok(responseDto);
        }
*/

        [HttpPost("{menuId}")]
        public async Task<ActionResult<AddDishToMenuResultDto>> AddDishToMenu(int menuId, [FromBody] AddDishToMenuDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new AddDishToMenuCommand(menuId, dto));

            if (result == null)
                return NotFound("Menu introuvable ou plat invalide.");

            if (result.DishAlreadyExists)
                return BadRequest(result);

            return Ok(result);
        }


        /*      // Supprimer un plat du menu
              [HttpDelete("{menuId}/{dishId}")]
              public async Task<ActionResult<Menu>> RemoveDishFromMenu(int menuId, int dishId)
              {
                  var menu = await _menuService.RemoveDishFromMenuAsync(menuId, dishId);
                  if (menu == null)
                  {
                      return NotFound();
                  }
                  return Ok(menu);
              }*/

        // Supprimer un plat du menu
        [HttpDelete("{menuId}/{dishId}")]
        public async Task<ActionResult<Menu>> RemoveDishFromMenu(int menuId, int dishId)
        {
            var result = await _mediator.Send(new RemoveDishFromMenuCommand(menuId, dishId));
            if (result == null)
                return NotFound();

            return Ok(result);
        }


        /*   // Supprimer un menu
           [HttpDelete("{id}")]
           public async Task<ActionResult> DeleteMenu(int id)
           {
               await _menuService.DeleteMenuAsync(id);
               return NoContent();
           }*/

        // Supprimer un menu
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenu(int id)
        {
            await _mediator.Send(new DeleteMenuCommand(id));
            return NoContent();
        }

        /*  // Obtenir un menu par ID
          [HttpGet("{id}")]
          public async Task<ActionResult<Menu>> GetMenu(int id)
          {
              var menu = await _menuService.GetMenuByIdAsync(id);
              if (menu == null)
              {
                  return NotFound();
              }
              return Ok(menu);
          }*/

        // GET api/menu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu(int id)
        {
            var menu = await _mediator.Send(new GetMenuByIdQuery(id));
            if (menu == null)
                return NotFound();
            return Ok(menu);
        }
        /*   // Obtenir tous les menus
           [HttpGet]
           public async Task<ActionResult<IEnumerable<GetAllMenusDto>>> GetMenus()
           {
               var menus = await _menuService.GetAllMenusAsync();
               return Ok(menus);
           }*/
        // GET api/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllMenusDto>>> GetMenus()
        {
            var menus = await _mediator.Send(new GetAllMenusQuery());
            return Ok(menus);
        }

        /*   // Sélectionner un menu comme "Menu du jour"
           [HttpPost("setMenuOfTheDay/{menuId}")]
           public async Task<IActionResult> SetMenuOfTheDay(int menuId)
           {
               var success = await _menuService.SetMenuOfTheDayAsync(menuId);
               if (success)
               {
                   return Ok("Menu of the day has been updated.");
               }
               return BadRequest("Could not update menu of the day.");
           }*/
        // POST api/menu/setMenuOfTheDay/{menuId}
        [HttpPost("setMenuOfTheDay/{menuId}")]
        public async Task<IActionResult> SetMenuOfTheDay(int menuId)
        {
            bool success = await _mediator.Send(new SetMenuOfTheDayCommand(menuId));
            if (success)
                return Ok("Menu of the day has been updated.");
            return BadRequest("Could not update menu of the day.");
        }
    }
}