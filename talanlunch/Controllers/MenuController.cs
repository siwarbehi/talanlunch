using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Interfaces;

namespace talanlunch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // Ajouter un menu
        [HttpPost]
        public async Task<IActionResult> AddMenuAsync([FromForm] MenuDto menuDto)
        {
            try
            {
                var createdMenu = await _menuService.AddMenuAsync(menuDto);
                return Ok(createdMenu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    ParamName = ex.ParamName
                });
            }
        }


        // Modifier la description du menu
        [HttpPut("{id}/description")]
        public async Task<IActionResult> UpdateMenuDescription (int id, [FromForm] string newDescription)
        {
            var result = await _menuService.UpdateMenuDescriptionAsync(id, newDescription);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Ajouter un plat au menu
        [HttpPost("{menuId}/add-dish/{dishId}")]
        public async Task<ActionResult<Menu>> AddDishToMenu(int menuId, int dishId)
        {
            var menu = await _menuService.AddDishToMenuAsync(menuId, dishId);
            if (menu == null)
            {
                return NotFound();
            }
            return Ok(menu);
        }

        // Supprimer un plat du menu
        [HttpDelete("{menuId}/remove-dish/{dishId}")]
        public async Task<ActionResult<Menu>> RemoveDishFromMenu(int menuId, int dishId)
        {
            var menu = await _menuService.RemoveDishFromMenuAsync(menuId, dishId);
            if (menu == null)
            {
                return NotFound();
            }
            return Ok(menu);
        }

        // Supprimer un menu
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenu(int id)
        {
            await _menuService.DeleteMenuAsync(id);
            return NoContent();
        }

        // Obtenir un menu par ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return Ok(menu);
        }

        // Obtenir tous les menus
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(menus);
        }
    }
}