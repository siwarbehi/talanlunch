using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos.User;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Enums;
using TalanLunch.Application.User.Queries;
using TalanLunch.Application.User.Commands;


namespace talanlunch.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;


        public UserController(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;

        }

        /*    [HttpGet("{userId}")]
            public async Task<IActionResult> GetUserById(int userId)
            {
                try
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    return Ok(user);
                }
                catch (ArgumentException ex)
                {
                    return NotFound(ex.Message);
                }
            }*/
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByIdQuery(userId));
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /* [HttpPatch("{userId}")]
         public async Task<IActionResult> UpdateUserProfile(int userId, [FromForm] UserDto userDto)
         {
             try
             {
                 var updatedUser = await _userService.UpdateUserProfileAsync(userId, userDto);
                 return Ok(updatedUser);
             }
             catch (ArgumentException ex)
             {
                 return NotFound(ex.Message);
             }
         }*/
        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromForm] UserDto userDto)
        {
            try
            {
                var updatedUser = await _mediator.Send(new UpdateUserProfileCommand(userId, userDto));
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
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
        }

    }

}

