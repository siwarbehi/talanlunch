using Microsoft.EntityFrameworkCore;
using System.Data;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;
using TalanLunch.Infrastructure.Data;

namespace TalanLunch.Infrastructure.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly TalanLunchDbContext _context;

        public UserRepository(TalanLunchDbContext context)
        {
            _context = context;
        }

        // Nouvelle méthode pour récupérer un utilisateur par son adresse email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
        }

  
        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();

                return true; 
            }
            catch (Exception ex)
            {
                
                return false; 
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

      

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetPendingCaterersAsync()
        {
            return await _context.Users
                .Where(u => u.UserRole == UserRole.CATERER && u.IsApproved == false)
                .ToListAsync();
        }

        public async Task<User?> GetCatererByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && u.UserRole == UserRole.CATERER && u.IsApproved == false);
        }

        // Mettre à jour traiteur (true=IsApproved )
        public async Task<bool> ApproveUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateUserDataAsync(User user)
        {
            // Recherche de l'utilisateur dans la base de données par son ID
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Utilisateur avec ID {user.UserId} non trouvé.");
            }

            // Mise à jour des champs de l'utilisateur
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.EmailAddress = user.EmailAddress;
            existingUser.ProfilePicture = user.ProfilePicture;

            // Sauvegarde des modifications dans la base de données
            await _context.SaveChangesAsync();
        }
        // Récupérer l'utilisateur via son RefreshToken
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        // Supprimer le RefreshToken lors du logout
        public async Task DeleteRefreshTokenAsync(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _context.SaveChangesAsync();
        }
        // Recherche un utilisateur par son token de réinitialisation
        public async Task<User?> GetByResetTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ResetToken == token);
        }

    }
}