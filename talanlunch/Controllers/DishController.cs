using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dishes.Commands;
using TalanLunch.Application.Dishes.Commands.AddDish;
using TalanLunch.Application.Dishes.Commands.UpdateDish;
using TalanLunch.Application.Dishes.Queries.GetAllDishes;
using TalanLunch.Application.Dishes.Queries.GetDishById;
using TalanLunch.Domain.Entities;

namespace Talanlunch.API.Controllers
{
    [Authorize]
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
            var createdDish = await _mediator.Send(command).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetDishById), new { id = createdDish.DishId }, createdDish);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDish(int id, [FromForm] UpdateDishCommand command)
        {
            command.DishId = id;
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return result == null ? NotFound($"Dish with id {id} not found.") : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAllDishes([FromQuery] GetAllDishesQuery query)
        {
            var dishes = await _mediator.Send(query).ConfigureAwait(false);
            return Ok(dishes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDishById([FromRoute] int id)
        {
            var query = new GetDishByIdQuery { DishId = id };
            var dish = await _mediator.Send(query).ConfigureAwait(false);
            return dish == null ? NotFound() : Ok(dish);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            await _mediator.Send(new DeleteDishCommand(id)).ConfigureAwait(false);
            return NoContent();
        }
    }
}
