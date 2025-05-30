using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalanLunch.Application.Auth.Commands.RegisterUser;
using TalanLunch.Application.Auth.Commands.LoginUser;
using TalanLunch.Application.Auth.Commands.Logout;
using TalanLunch.Application.Auth.Commands.ForgotPassword;
using TalanLunch.Application.Auth.Commands.ResetPassword;
using TalanLunch.Application.Auth.Commands.RefreshToken;
using TalanLunch.Application.Auth.Common;
using Microsoft.AspNetCore.Authorization;

namespace Talanlunch.API.Controllers
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
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return result is null ? BadRequest("Invalid email or password.") : Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return (result is null || string.IsNullOrEmpty(result.AccessToken) || string.IsNullOrEmpty(result.RefreshToken))
                ? Unauthorized("Invalid refresh token.")
                : Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return !result ? BadRequest("Échec de la déconnexion.") : Ok("Déconnexion réussie.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return result ? Ok("Un e-mail de réinitialisation a été envoyé.") : NotFound("Aucun utilisateur trouvé avec cet email.");
        }

        [HttpPost("reset-password/{token}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string token, [FromBody] ResetPasswordCommand command)
        {
            command.Token = token;
            var result = await _mediator.Send(command).ConfigureAwait(false);
            return result
                ? Ok(new { Message = "Mot de passe réinitialisé avec succès." })
                : StatusCode(500, new { Message = "Une erreur est survenue lors de la réinitialisation du mot de passe." });
        }
    }
}
