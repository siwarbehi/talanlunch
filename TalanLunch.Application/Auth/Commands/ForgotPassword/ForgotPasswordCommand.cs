using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
