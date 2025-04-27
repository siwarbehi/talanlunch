using TalanLunch.Application.Dtos;
using TalanLunch.Application.DTOs;

namespace TalanLunch.Application.Interfaces
{
    public interface IAuthService
    {
        // Méthode pour enregistrer un utilisateur
        Task<string> RegisterUserAsync(RegisterUserDto registerUserDto, bool isCaterer);

        // Méthode pour rafraîchir les tokens (AccessToken et RefreshToken)
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
        Task<TokenResponseDto?> LoginAsync(LoginDto request);
        Task<bool> LogoutAsync(string refreshToken);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);



    }
}
