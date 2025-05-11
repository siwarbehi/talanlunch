using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<bool>
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
