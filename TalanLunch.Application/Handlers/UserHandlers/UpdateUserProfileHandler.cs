using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Common;
using TalanLunch.Application.Dtos.User;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Services;
using TalanLunch.Application.User.Commands;

namespace TalanLunch.Application.Users.Handlers
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public UpdateUserProfileHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<UserDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException("Utilisateur non trouvé");
            }

            bool isUpdated = false;

            // Mise à jour des champs
            if (!string.IsNullOrEmpty(request.UserDto.FirstName) && request.UserDto.FirstName != user.FirstName)
            {
                user.FirstName = request.UserDto.FirstName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(request.UserDto.LastName) && request.UserDto.LastName != user.LastName)
            {
                user.LastName = request.UserDto.LastName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(request.UserDto.PhoneNumber) && request.UserDto.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = request.UserDto.PhoneNumber;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(request.UserDto.EmailAddress) && request.UserDto.EmailAddress != user.EmailAddress)
            {
                if (!IsValidEmail(request.UserDto.EmailAddress))
                {
                    throw new ArgumentException("Adresse email invalide.");
                }
                user.EmailAddress = request.UserDto.EmailAddress;
                isUpdated = true;
            }

            // Mise à jour de la photo de profil
            if (request.UserDto.ProfilePicture != null && request.UserDto.ProfilePicture.Length > 0)
            {
                user.ProfilePicture = await SaveProfileImageAsync(request.UserDto.ProfilePicture);
                isUpdated = true;
            }

            // Mise à jour du mot de passe
            if (!string.IsNullOrEmpty(request.UserDto.UpdatedPassword))
            {
                user.HashedPassword = AuthCommon.HashPassword(request.UserDto.UpdatedPassword);
                isUpdated = true;
            }

            if (isUpdated)
            {
                await _userRepository.UpdateUserDataAsync(user);
            }

            // Mapper l'entité vers UserDto pour la réponse
            return _mapper.Map<UserDto>(user);
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

        // Sauvegarde de l'image de profil
        private async Task<string> SaveProfileImageAsync(IFormFile profilePicture)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{profilePicture.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            return uniqueFileName;
        }
    }
}
