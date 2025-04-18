﻿using Microsoft.AspNetCore.Http;
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

        // POST: api/Menu
        [HttpPost]
        public async Task<IActionResult> AddMenuAsync([FromBody] MenuDto menuDto)
        {
            try
            {
                if (menuDto == null)
                    return BadRequest("Le DTO du menu ne peut pas être null.");

                var createdMenu = await _menuService.AddMenuAsync(menuDto);
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenu.MenuId }, createdMenu);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    ParamName = ex.ParamName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Une erreur inattendue est survenue.",
                    Error = ex.Message
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
        public async Task<ActionResult<Menu>> AddDishToMenu(int menuId, int dishId, [FromQuery] int quantity = 1)
        {
            var result = await _menuService.AddDishToMenuAsync(menuId, dishId, quantity);

            if (result.Item1 == null)
            {
                return NotFound();
            }

            if (result.Item2)
            {
                return BadRequest("Le plat est déjà présent dans ce menu.");
            }

            return Ok(result.Item1);
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
        public async Task<ActionResult<IEnumerable<GetAllMenusDto>>> GetMenus()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(menus);
        }

        // Action pour récupérer tous les MenuId
        [HttpGet("menu-ids")]
        public IActionResult GetAllMenuIds()
        {
            var menuIds = _menuService.GetAllMenuIds();

            if (menuIds == null || menuIds.Count == 0)
            {
                return NotFound(new { message = "Aucun menu trouvé." });
            }

            return Ok(menuIds);
        }
        // Action pour récupérer les plats associés à un menu donné
        [HttpGet("menu/{menuId}/dishes")]
        public IActionResult GetDishesByMenuId(int menuId)
        {
            var dishIds = _menuService.GetDishIdsForMenu(menuId);

            if (dishIds == null || !dishIds.Any())
            {
                return NotFound(new { message = "Aucun plat trouvé pour ce menu." });
            }

            return Ok(dishIds);
        }
        // Endpoint pour sélectionner un menu comme "Menu du jour"
        [HttpPost("setMenuOfTheDay/{menuId}")]
        public async Task<IActionResult> SetMenuOfTheDay(int menuId)
        {
            var success = await _menuService.SetMenuOfTheDayAsync(menuId);
            if (success)
            {
                return Ok("Menu of the day has been updated.");
            }
            return BadRequest("Could not update menu of the day.");
        }
    }
}