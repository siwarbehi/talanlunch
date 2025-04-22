using Microsoft.AspNetCore.Mvc;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Services;
namespace talanlunch.Controllers
{
    [ApiController]
    [Route("api/dish")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        // Creation plat
        [HttpPost]
        public async Task<IActionResult> AddDish([FromForm] DishDto dishDto)
        {
            if (dishDto == null)
                return BadRequest("Dish data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdDish = await _dishService.AddDishAsync(dishDto);
                return CreatedAtAction(nameof(GetDishById), new { id = createdDish.DishId }, createdDish);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur interne est survenue.", error = ex.Message });
            }
        }



        // Modifier un plat
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDish(int id, [FromForm] DishUpdateDto updatedDish, IFormFile? dishPhoto)
        {
            if (updatedDish == null)
                return BadRequest("Dish data is null.");

        
            var existingDish = await _dishService.GetDishByIdAsync(id);
            if (existingDish == null)
                return NotFound($"Dish with id {id} not found.");

            var updatedDishEntity = await _dishService.UpdateDishAsync(existingDish, updatedDish, dishPhoto);

            return Ok(updatedDishEntity); 
        }



        // GET: api/dish/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _dishService.GetAllDishesAsync();
            return Ok(dishes);  // 200 OK with the list of dishes
        }
       
        // GET: api/dish/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDishById(int id)
        {
            var dish = await _dishService.GetDishByIdAsync(id);
            return dish == null ? NotFound() : Ok(dish);
        }

        // DELETE: api/dish/delete/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                await _dishService.DeleteDishAsync(id);
                return NoContent(); // 204
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Dish with ID {id} not found.");
            }
            catch (Exception ex)
            {
                // Gérer d'autres erreurs possibles
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
     
        [HttpPost("rate")]
        public async Task<IActionResult> RateDish([FromBody] RateDishDto dto)
        {
            try
            {
                await _dishService.RateDishAsync(dto);
                return Ok(new { message = "Merci pour votre note !" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
