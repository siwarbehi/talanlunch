using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Dtos.Auth;
using TalanLunch.Application.Auth;

namespace talanlunch.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;


        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto, [FromQuery] bool isCaterer)
        {
            if (registerUserDto == null)
                return BadRequest("Les données d'inscription sont manquantes.");

            var command = new RegisterUserCommand(registerUserDto, isCaterer);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new LoginCommand(request));
            if (result is null)
                return BadRequest("Invalid username or password.");

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(request));

            if (result is null || string.IsNullOrEmpty(result.AccessToken) || string.IsNullOrEmpty(result.RefreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto logoutRequest)
        {
            if (logoutRequest == null || string.IsNullOrEmpty(logoutRequest.RefreshToken))
            {
                return BadRequest("Refresh token manquant.");
            }

            var result = await _mediator.Send(new LogoutCommand(logoutRequest.RefreshToken));

            if (!result)
            {
                return BadRequest("Échec de la déconnexion.");
            }

            return Ok("Déconnexion réussie.");
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("L'email est requis.");

            var command = new ForgotPasswordCommand(request.Email);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("Aucun utilisateur trouvé avec cet email.");

            return Ok("Un e-mail de réinitialisation a été envoyé.");
        }

        [HttpPost("reset-password/{token}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string token, [FromBody] ResetRequest request)
        {
            if (string.IsNullOrEmpty(request.NewPassword))
                return BadRequest(new { Message = "Le mot de passe ne peut pas être vide." });

            var command = new ResetPasswordCommand(token, request.NewPassword);
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Mot de passe réinitialisé avec succès." });

            return StatusCode(500, new { Message = "Une erreur est survenue lors de la réinitialisation du mot de passe." });
        }


    }
}