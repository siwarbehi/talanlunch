using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<Domain.Entities.User>> GetAllUsersAsync();
        Task<IEnumerable<Domain.Entities.User>> GetUsersByRoleAsync(UserRole role);
        Task DeleteUserAsync(int userId);

    }
}
