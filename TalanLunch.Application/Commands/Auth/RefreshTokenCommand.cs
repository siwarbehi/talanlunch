using MediatR;
using TalanLunch.Application.Dtos.Auth;

namespace TalanLunch.Application.Features.Auth.Commands
{
    public class RefreshTokenCommand : IRequest<TokenResponseDto?>
    {
        public RefreshTokenRequestDto Request { get; }

        public RefreshTokenCommand(RefreshTokenRequestDto request)
        {
            Request = request;
        }
    }
}
