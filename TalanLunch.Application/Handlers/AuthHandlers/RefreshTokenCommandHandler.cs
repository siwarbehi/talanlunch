using MediatR;
using TalanLunch.Application.Common;
using TalanLunch.Application.Dtos.Auth;
using TalanLunch.Application.Features.Auth.Commands;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Features.Auth.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponseDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthCommon _authCommon;

        public RefreshTokenCommandHandler(IUserRepository userRepository, AuthCommon authCommon)
        {
            _userRepository = userRepository;
            _authCommon = authCommon;
        }

        public async Task<TokenResponseDto?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Request;

            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var tokenResponse = await _authCommon.CreateTokenResponseAsync(user);
            tokenResponse.IsApproved = user.IsApproved;

            return tokenResponse;
        }
    }
}
