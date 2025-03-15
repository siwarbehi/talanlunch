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
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task DeleteUserAsync(int userId);
        Task<List<User>> GetPendingCaterersAsync();
        Task<User?> GetCatererByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<List<User>> GetCaterersByIdsAsync(List<int> catererIds);
        Task<bool> UpdateMultipleCaterersAsync(List<User> caterers);
        Task UpdateUserDataAsync(User user);


    }
}
