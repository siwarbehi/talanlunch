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
        public async Task<IActionResult> GetUserById([FromRoute] GetUserByIdQuery query)
        {
            try
            {
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


        /* [HttpGet]
         public async Task<IActionResult> GetUsersByRole([FromQuery] string role = null )
         {
             var users = role?.ToLower() switch
             {
                 null => await _userService.GetAllUsersAsync(),
                 "collaborators" => await _userService.GetUsersByRoleAsync(UserRole.COLLABORATOR),
                 "caterers" => await _userService.GetUsersByRoleAsync(UserRole.CATERER),
                 _ => null
             };

             if (users == null)
                 return BadRequest("Rôle invalide. Utilisez 'collaborators' ou 'caterers'.");

             return Ok(users);
         }

         // Supprimer un user
         [HttpDelete("{id}")]
         public async Task<ActionResult> DeleteUser(int id)
         {
             await _userService.DeleteUserAsync(id);
             return NoContent();
         }*/

    }

}

