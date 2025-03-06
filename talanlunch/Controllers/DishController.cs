using Microsoft.AspNetCore.Mvc;
using TalanLunch.Domain.Entites;
using TalanLunch.Application.Interfaces;
namespace talanlunch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        // POST: api/dish/add
        [HttpPost("add")]
        public async Task<IActionResult> AddDish([FromBody] Dish newDish)
        {
            if (newDish == null)
                return BadRequest("Dish data is null.");

            var createdDish = await _dishService.AddDishAsync(newDish);
            return CreatedAtAction(nameof(GetDishById), new { id = createdDish.DishId }, createdDish);
        }

        // PUT: api/dish/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] Dish updatedDish)
        {
            if (updatedDish == null || updatedDish.DishId != id)
                return BadRequest("Dish ID mismatch.");

            var dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
                return NotFound($"Dish with ID {id} not found.");

            await _dishService.UpdateDishAsync(updatedDish);
            return NoContent();  // 204 No Content (successful update)
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
        public async Task<IActionResult> GetDishById(int id)
        {
            var dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
                return NotFound($"Dish with ID {id} not found.");

            return Ok(dish);  // 200 OK with the dish details
        }

        // DELETE: api/dish/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null)
                return NotFound($"Dish with ID {id} not found.");

            await _dishService.DeleteDishAsync(id);
            return NoContent();  // 204 No Content (successful deletion)
        }
    }
}
