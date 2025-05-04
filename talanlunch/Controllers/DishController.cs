using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Commands.Dish;
using TalanLunch.Application.Dtos.Dish;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Queries.Dish;
using TalanLunch.Domain.Entities;
using MediatR;

namespace talanlunch.Controllers
{
    [ApiController]
    [Route("api/dish")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        private readonly IMediator _mediator;


        public DishController(IDishService dishService, IMediator mediator)
        {
            _dishService = dishService;
            _mediator = mediator;

        }
        /*  //creation d un plat 
          [HttpPost]
          public async Task<IActionResult> AddDish(DishDto dishDto)
          {
              if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState);
              }

              var createdDish = await _dishService.AddDishAsync(dishDto);

              return CreatedAtAction(nameof(GetDishById), new { id = createdDish.DishId }, createdDish);
          }*/
        // POST api/dish
        [HttpPost]
        public async Task<IActionResult> AddDish([FromForm] DishDto dishDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDish = await _mediator.Send(new AddDishCommand(dishDto));
            return CreatedAtAction(nameof(GetDishById),
                new { id = createdDish.DishId },
                createdDish);
        }

        //modifier un plat 

        /*  [HttpPatch("{id}")]
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
          }*/

        // PATCH api/dish/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDish(
            int id,
            [FromForm] DishUpdateDto updatedDish,
            IFormFile? dishPhoto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(
                new UpdateDishCommand(id, updatedDish, dishPhoto));

            if (result == null)
                return NotFound($"Dish with id {id} not found.");

            return Ok(result);
        }

        /* // GET: api/dish
         [HttpGet]
         public async Task<IActionResult> GetAllDishes()
         {
             var dishes = await _dishService.GetAllDishesAsync();
             return Ok(dishes);  
         }*/
        // GET api/dish
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAllDishes()
        {
            var dishes = await _mediator.Send(new GetAllDishesQuery());
            return Ok(dishes);
        }

        /*  // GET: api/dish/{id}
          [HttpGet("{id}")]
          public async Task<ActionResult<Dish>> GetDishById(int id)
          {
              var dish = await _dishService.GetDishByIdAsync(id);
              return dish == null ? NotFound() : Ok(dish);
          }*/

        // GET api/dish/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDishById(int id)
        {
            var dish = await _mediator.Send(new GetDishByIdQuery(id));
            if (dish == null)
                return NotFound();
            return Ok(dish);
        }
        /* // DELETE: api/dish/delete/{id}
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
         }*/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                await _mediator.Send(new DeleteDishCommand(id));
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
