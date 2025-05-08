using MediatR;
using TalanLunch.Application.Dtos.Auth;

namespace TalanLunch.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<TokenResponseDto?>
    {
        public LoginDto LoginDto { get; }

        public LoginCommand(LoginDto dto)
        {
            LoginDto = dto;
        }
    }
}
