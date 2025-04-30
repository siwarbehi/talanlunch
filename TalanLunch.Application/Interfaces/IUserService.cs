using TalanLunch.Application.Dtos.User;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> UpdateUserProfileAsync(int userId, UserDto userDto);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task DeleteUserAsync(int userId);

    }
}
