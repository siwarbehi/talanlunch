using MediatR;
using TalanLunch.Application.Common;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Auth.Commands.RefreshToken
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
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var tokenResponse = await _authCommon.CreateTokenResponseAsync(user);
            tokenResponse.IsApproved = user.IsApproved;

            return tokenResponse;
        }
    }
}
