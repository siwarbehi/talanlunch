using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
