using MediatR;
using TalanLunch.Application.Dtos.Auth;

namespace TalanLunch.Application.Auth
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
