using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // Vérifier si un utilisateur existe par son email
        Task<bool> UserExistsAsync(string email);

        // Ajouter un nouvel utilisateur
        Task AddUserAsync(User user);

        // Mettre à jour un utilisateur existant
        Task UpdateUserAsync(User user);

        // Récupérer tous les utilisateurs
        Task<IEnumerable<User>> GetAllUsersAsync();

        // Récupérer les utilisateurs par leur rôle
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);

        // Supprimer un utilisateur par son identifiant
        Task DeleteUserAsync(int userId);

        // Récupérer les traiteurs non approuvés
        Task<List<User>> GetPendingCaterersAsync();

        // Récupérer un traiteur par son identifiant
        Task<User?> GetCatererByIdAsync(int id);

        // Approuver un utilisateur (mettre à jour IsApproved)
        Task<bool> ApproveUserAsync(User user);

        // Récupérer plusieurs traiteurs non approuvés par leurs identifiants
        Task<List<User>> GetCaterersByIdsAsync(List<int> catererIds);

        // Mettre à jour plusieurs traiteurs (mettant à jour IsApproved)
        Task<bool> UpdateMultipleCaterersAsync(List<User> caterers);

        // Mettre à jour les données d'un utilisateur
        Task UpdateUserDataAsync(User user);
    }
}
