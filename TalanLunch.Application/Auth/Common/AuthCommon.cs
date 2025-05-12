using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Auth.Common
{
    public class AuthCommon
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthCommon(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<Domain.Entities.User>();
            return passwordHasher.HashPassword(null, password);
        }

        public async Task<TokenResponseDto> CreateTokenResponseAsync(Domain.Entities.User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être nul.");
            }

            var accessToken = CreateAccessToken(user);
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

       
        private async Task<string> GenerateAndSaveRefreshTokenAsync(Domain.Entities.User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateUserAsync(user);
            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

     
        private string CreateAccessToken(Domain.Entities.User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var secret = _configuration.GetValue<string>("AppSettings:Token");
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("Le secret du token est introuvable dans la configuration.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
