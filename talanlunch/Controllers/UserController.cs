using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Users.Commands;
using TalanLunch.Application.Users.Queries.GetUserById;


namespace talanlunch.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;


        public UserController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] int userId)
        {
            try
            {
                var query = new GetUserByIdQuery(userId);
                var user = await _mediator.Send(query);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromForm] UpdateUserProfileCommand command)
        {
            if (userId != command.UserId)
            {
                return BadRequest("ID utilisateur non cohérent.");
            }

            try
            {
                var updatedUser = await _mediator.Send(command);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }      
    }

}

