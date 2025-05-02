using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos.Dish;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
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
        //creation d un plat 
        [HttpPost]
        public async Task<IActionResult> AddDish(DishDto dishDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdDish = await _dishService.AddDishAsync(dishDto);

            return CreatedAtAction(nameof(GetDishById), new { id = createdDish.DishId }, createdDish);
        }

        //modifier un plat 

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDish(int id, [FromForm] DishUpdateDto updatedDish, IFormFile? dishPhoto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (updatedDish == null)
            {
                return BadRequest("Dish data is required.");
            }

            var existingDish = await _dishService.GetDishByIdAsync(id);
            if (existingDish == null)
            {
                return NotFound($"Dish with id {id} not found.");
            }
            var updatedDishEntity = await _dishService.UpdateDishAsync(existingDish, updatedDish, dishPhoto);

            return Ok(updatedDishEntity);
        }



        // GET: api/dish
        [HttpGet]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _dishService.GetAllDishesAsync();
            return Ok(dishes);  
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
                return NoContent(); 
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Dish with ID {id} not found.");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
     
       

    }
}
