using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Dtos.User;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _uploadsFolder;


        public UserService(IUserRepository userRepository )
        {
            _userRepository = userRepository;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UserProfilePictures");

            // Ensure the upload folder exists
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }
        public async Task<IEnumerable<Domain.Entities.User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }


        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }
        public async Task<IEnumerable<Domain.Entities.User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _userRepository.GetUsersByRoleAsync(role);
        }


    }
}
