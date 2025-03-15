using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Services;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace talanlunch.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPut("{userId}")]
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
        [HttpGet("collaborators")]
        public async Task<IActionResult> ListCollaborators()
        {
            var collaborators = await _userService.GetUsersByRoleAsync(UserRole.COLLABORATOR);
            return Ok(collaborators);
        }

        [HttpGet("caterers")]
        public async Task<IActionResult> ListCaterers()
        {
            var caterers = await _userService.GetUsersByRoleAsync(UserRole.CATERER);
            return Ok(caterers);
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

