using MediatR;
using TalanLunch.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Auth.Common;

namespace TalanLunch.Application.Auth.Commands.LoginUser
{
    public class LoginCommandHandler : IRequestHandler<LoginUserCommand, TokenResponseDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthCommon _authCommon;

        public LoginCommandHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _authCommon = new AuthCommon(userRepository, configuration);
        }

        public async Task<TokenResponseDto?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.EmailAddress);
            if (user is null)
                return null;

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            var tokenResponse = await _authCommon.CreateTokenResponseAsync(user);
            tokenResponse.IsApproved = user.IsApproved;
            tokenResponse.UserRole = user.UserRole;


            return tokenResponse;
        }
    }
}
