﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users
                                 .Where(user => user.UserRole == role)
                                 .ToListAsync();
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
        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        // Récupérer plusieurs traiteurs non approuvees par leurs ID
        public async Task<List<User>> GetCaterersByIdsAsync(List<int> catererIds)
        {
            return await _context.Users.Where(c => catererIds.Contains(c.UserId) && !c.IsApproved).ToListAsync();
        }
        // Mettre à jour plusieurs traiteurs  (true=IsApproved )
        public async Task<bool> UpdateMultipleCaterersAsync(List<User> caterers)
        {
            _context.Users.UpdateRange(caterers);
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
    }
}


