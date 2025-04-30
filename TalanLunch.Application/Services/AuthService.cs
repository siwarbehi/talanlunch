using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TalanLunch.Application.Dtos.Auth;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;


namespace TalanLunch.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;


        public AuthService(IUserRepository userRepository, IConfiguration configuration, IMailService mailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mailService = mailService;
        }

        public async Task<string> RegisterUserAsync(RegisterUserDto registerUserDto, bool isCaterer)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(registerUserDto.EmailAddress);
            if (existingUser != null)
            {
                return "Cet email est déjà utilisé.";
            }

            var user = new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                EmailAddress = registerUserDto.EmailAddress,
                PhoneNumber = registerUserDto.PhoneNumber,
                UserRole = isCaterer ? UserRole.CATERER : UserRole.COLLABORATOR,
                IsApproved = !isCaterer, 
            };

            var hashedPassword = HashPassword(registerUserDto.Password);
            user.HashedPassword = hashedPassword;

            bool isAdded = await _userRepository.AddUserAsync(user);

            if (!isAdded)
            {
                return "Une erreur s'est produite lors de l'enregistrement de l'utilisateur. Veuillez réessayer.";
            }

            if (isCaterer)
            {
                return "Votre demande d'inscription en tant que traiteur a été enregistrée. Un administrateur doit approuver votre compte.";
            }

            return "Inscription réussie.";
        }

        public static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(null, password); 
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.EmailAddress);
            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.HashedPassword, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var tokenResponse = await CreateTokenResponse(user);

            // Ajouter IsApproved dans la réponse
            tokenResponse.IsApproved = user.IsApproved;

            return tokenResponse;
        }


        // Méthode pour créer un TokenResponse contenant le token d'accès et le refresh token
        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "L'utilisateur ne peut pas être nul.");
            }

            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        // Méthode pour générer et sauvegarder un refresh token
        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
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

        // Méthode pour créer un token JWT d'accès
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        // Méthode pour rafraîchir les tokens (access token et refresh token)
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        // Méthode pour valider un refresh token
        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            // Récupérer l'utilisateur via son RefreshToken
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null) return false;

            // Supprimer le RefreshToken et sa date d'expiration
            await _userRepository.DeleteRefreshTokenAsync(user);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            // Générer un token unique
            string resetToken = Guid.NewGuid().ToString();
            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserAsync(user);

            // Envoyer l'email via MailService
            await _mailService.SendPasswordResetEmailAsync(user, resetToken);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _userRepository.GetByResetTokenAsync(token);

            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return false; // Token invalide ou expiré
            }

            // Hachage du mot de passe
            var hashedPassword = HashPassword(newPassword);

            user.HashedPassword = hashedPassword;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateUserAsync(user);
            return true; // Mot de passe réinitialisé avec succès
        }
    }
}
