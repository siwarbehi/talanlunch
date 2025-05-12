using MediatR;
using System.ComponentModel.DataAnnotations;
using TalanLunch.Application.Auth.Common;

namespace TalanLunch.Application.Auth.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<TokenResponseDto?>
    {
        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
