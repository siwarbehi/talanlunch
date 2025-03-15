using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
   

        public interface IAuthRepository
        {
            Task<User?> GetUserByEmailAsync(string email);
            Task AddUserAsync(User user);
            Task SaveChangesAsync();
        }
    
}
