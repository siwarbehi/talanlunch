using MediatR;
using TalanLunch.Application.Dtos.Auth;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TalanLunch.Application.Auth;

namespace TalanLunch.Application.Features.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponseDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthCommon _authCommon;

        public LoginCommandHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _authCommon = new AuthCommon(userRepository, configuration);
        }

        public async Task<TokenResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDto = request.LoginDto;

            var user = await _userRepository.GetUserByEmailAsync(loginDto.EmailAddress);
            if (user is null)
                return null;

            var passwordHasher = new PasswordHasher<Domain.Entities.User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var tokenResponse = await _authCommon.CreateTokenResponseAsync(user);
            tokenResponse.IsApproved = user.IsApproved;

            return tokenResponse;
        }
    }
}
