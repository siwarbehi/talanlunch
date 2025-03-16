using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.DTOs;
using TalanLunch.Application.Dtos;
using TalanLunch.Application.Services;

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
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null)
                return BadRequest("Invalid username or password.");

            return Ok(result);
        }
        /// Refresh the access token and refresh token using the provided refresh token.

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);

            // Vérification si le résultat est null ou si les tokens sont invalides
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            // Retourne les nouveaux tokens si valides
            return Ok(result);
        }

    }
}