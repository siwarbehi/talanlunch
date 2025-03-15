using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entities;
using TalanLunch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TalanLunch.Application.Interfaces;


namespace TalanLunch.Infrastructure.Repos
{
    public class AuthRepository : IAuthRepository

    {
        private readonly TalanLunchDbContext _context;

        public AuthRepository(TalanLunchDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}