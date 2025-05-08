using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Interfaces
{
    public interface IUserRepository
    {
        // Récupérer un utilisateur par son adresse email
        Task<Domain.Entities.User?> GetUserByEmailAsync(string email);

        // Récupérer un utilisateur par son identifiant
        Task<Domain.Entities.User?> GetUserByIdAsync(int userId);

        // Vérifier si un utilisateur existe par son email
        Task<bool> UserExistsAsync(string email);

        // Ajouter un nouvel utilisateur
        Task<bool> AddUserAsync(Domain.Entities.User user);

        // Mettre à jour un utilisateur existant
        Task UpdateUserAsync(Domain.Entities.User user);

        // Récupérer tous les utilisateurs
        Task<IEnumerable<Domain.Entities.User>> GetAllUsersAsync();

        // Récupérer les utilisateurs par leur rôle
        Task<IEnumerable<Domain.Entities.User>> GetUsersByRoleAsync(UserRole role);

        // Supprimer un utilisateur par son identifiant
        Task DeleteUserAsync(int userId);

        // Récupérer les traiteurs non approuvés
        Task<List<Domain.Entities.User>> GetPendingCaterersAsync();

        // Récupérer un traiteur par son identifiant
        Task<Domain.Entities.User?> GetCatererByIdAsync(int id);

        // Approuver un utilisateur (mettre à jour IsApproved)
        Task<bool> ApproveUserAsync(Domain.Entities.User user);

        // Mettre à jour les données d'un utilisateur
        Task UpdateUserDataAsync(Domain.Entities.User user);
        Task<Domain.Entities.User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task DeleteRefreshTokenAsync(Domain.Entities.User user);
        Task<Domain.Entities.User> GetByResetTokenAsync(string token);

    }
}
