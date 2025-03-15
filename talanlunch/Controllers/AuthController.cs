using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.DTOs;

namespace talanlunch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto registerUserDto, [FromQuery] bool isCaterer)
        {
            if (registerUserDto == null)
            {
                return BadRequest("Les données d'inscription sont manquantes.");
            }

            string result = await _authService.RegisterUserAsync(registerUserDto, isCaterer);
            return Ok(result);
        }
      
    }
}