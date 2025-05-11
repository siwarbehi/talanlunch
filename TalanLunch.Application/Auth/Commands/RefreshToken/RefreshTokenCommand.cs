using MediatR;

namespace TalanLunch.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<TokenResponseDto?>
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;

        public RefreshTokenCommand(int userId, string refreshToken)
        {
            UserId = userId;
            RefreshToken = refreshToken;
        }
    }
}
