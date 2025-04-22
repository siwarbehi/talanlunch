using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Services;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace talanlunch.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
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
        }

        [HttpPatch("{userId}")]
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

