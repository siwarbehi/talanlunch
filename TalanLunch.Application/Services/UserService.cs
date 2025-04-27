using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Dtos;
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

        public async Task<User> UpdateUserProfileAsync(int userId, UserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Utilisateur non trouvé");
            }

            bool isUpdated = false;

            // Mise à jour des champs uniquement si une nouvelle valeur est fournie
            if (!string.IsNullOrEmpty(userDto.FirstName) && userDto.FirstName != user.FirstName)
            {
                user.FirstName = userDto.FirstName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(userDto.LastName) && userDto.LastName != user.LastName)
            {
                user.LastName = userDto.LastName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(userDto.PhoneNumber) && userDto.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = userDto.PhoneNumber;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(userDto.EmailAddress) && userDto.EmailAddress != user.EmailAddress)
            {
                if (!IsValidEmail(userDto.EmailAddress))
                {
                    throw new ArgumentException("Adresse email invalide.");
                }
                user.EmailAddress = userDto.EmailAddress;
                isUpdated = true;
            }

            // Mise à jour de la photo uniquement si une nouvelle image est fournie
            if (userDto.ProfilePicture != null && userDto.ProfilePicture.Length > 0)
            {
                user.ProfilePicture = await SaveProfileImageAsync(userDto.ProfilePicture);
                isUpdated = true;
            }

            // Gestion du mot de passe mis à jour
            if (!string.IsNullOrEmpty(userDto.UpdatedPassword))
            {
                // Appel de la méthode HashPassword d'AuthService pour hacher le mot de passe
                user.HashedPassword = AuthService.HashPassword(userDto.UpdatedPassword);
                isUpdated = true;
            }

            // Mise à jour uniquement si des changements ont été effectués
            if (isUpdated)
            {
                await _userRepository.UpdateUserDataAsync(user);
            }

            return user;
        }

        // Vérification de la validité d’un email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Utilisateur non trouvé");
            }
            return user;
        }

        private async Task<string> SaveProfileImageAsync(IFormFile profilePicture)
        {
            // Obtenez le répertoire wwwroot/uploads
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Assurez-vous que le répertoire existe
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Générer un nom de fichier unique pour l'image de profil
            string uniqueFileName = $"{Guid.NewGuid()}_{profilePicture.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Sauvegarder l'image de profil sur le disque
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            // Retourner le nom du fichier unique pour l'enregistrement
            return uniqueFileName;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }


        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _userRepository.GetUsersByRoleAsync(role);
        }


    }
}
