using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Interfaces
{
    public interface IUserRepository
    {
        // Récupérer un utilisateur par son adresse email
        Task<User?> GetUserByEmailAsync(string email);

        // Récupérer un utilisateur par son identifiant
        Task<User?> GetUserByIdAsync(int userId);

        // Ajouter un nouvel utilisateur
        Task<bool> AddUserAsync(User user);

        // Mettre à jour un utilisateur existant
        Task UpdateUserAsync(User user);

        // Récupérer tous les utilisateurs
        Task<IEnumerable<User>> GetAllUsersAsync();

        // Supprimer un utilisateur par son identifiant
        Task DeleteUserAsync(int userId);

        // Récupérer les traiteurs non approuvés
        Task<List<User>> GetPendingCaterersAsync();

        // Récupérer un traiteur par son identifiant
        Task<User?> GetCatererByIdAsync(int id);

        // Approuver un utilisateur (mettre à jour IsApproved)
        Task<bool> ApproveUserAsync(User user);

        // Mettre à jour les données d'un utilisateur
        Task UpdateUserDataAsync(User user);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task DeleteRefreshTokenAsync(User user);
        Task<User> GetByResetTokenAsync(string token);

    }
}
