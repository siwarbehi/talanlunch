using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Auth;
using TalanLunch.Application.Auth.Commands.RegisterUser;
using TalanLunch.Application.Auth.Commands.LoginUser;
using TalanLunch.Application.Auth.Commands.Logout;
using TalanLunch.Application.Auth.Commands.ForgotPassword;
using TalanLunch.Application.Auth.Commands.ResetPassword;
using TalanLunch.Application.Auth.Commands.RefreshToken;

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
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginUserCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            if (result is null)
                return BadRequest("Invalid email or password.");

            return Ok(result);
        }



        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            // Appeler le handler avec le RefreshTokenCommand
            var result = await _mediator.Send(command);

            if (result is null || string.IsNullOrEmpty(result.AccessToken) || string.IsNullOrEmpty(result.RefreshToken))
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest("Échec de la déconnexion.");
            }

            return Ok("Déconnexion réussie.");
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound("Aucun utilisateur trouvé avec cet email.");

            return Ok("Un e-mail de réinitialisation a été envoyé.");
        }

        [HttpPost("reset-password/{token}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string token, [FromBody] ResetPasswordCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.Token = token;

            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Mot de passe réinitialisé avec succès." });

            return StatusCode(500, new { Message = "Une erreur est survenue lors de la réinitialisation du mot de passe." });
        }


    }
}