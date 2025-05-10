using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dishes.Commands;
using TalanLunch.Application.Dishes.Commands.AddDish;
using TalanLunch.Application.Dishes.Commands.UpdateDish;
using TalanLunch.Application.Dishes.Queries.GetAllDishes;
using TalanLunch.Application.Dishes.Queries.GetDishById;
using TalanLunch.Domain.Entities;

namespace talanlunch.Controllers
{
    [ApiController]
    [Route("api/dish")]
    public class DishController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DishController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpPost]
        public async Task<IActionResult> AddDish([FromForm] AddDishCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDish = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetDishById),new { id = createdDish.DishId },createdDish);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDish(int id, [FromForm] UpdateDishCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.DishId = id;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"Dish with id {id} not found.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAllDishes([FromQuery] GetAllDishesQuery query)
        {
            var dishes = await _mediator.Send(query);
            return Ok(dishes);
        }
 
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDishById([FromRoute] int id)
        {
            var query = new GetDishByIdQuery { DishId = id };
            var dish = await _mediator.Send(query);
            if (dish == null)
                return NotFound();

            return Ok(dish);
        }

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
