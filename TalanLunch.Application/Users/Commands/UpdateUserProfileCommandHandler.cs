using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TalanLunch.Application.Auth.Common;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Users.Commands;

namespace TalanLunch.Application.Users.Handlers
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserProfileHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UpdateUserProfileCommandResult> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException("Utilisateur non trouvé");
            }

            bool isUpdated = false;

            if (!string.IsNullOrEmpty(request.FirstName) && request.FirstName != user.FirstName)
            {
                user.FirstName = request.FirstName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(request.LastName) && request.LastName != user.LastName)
            {
                user.LastName = request.LastName;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = request.PhoneNumber;
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(request.EmailAddress) && request.EmailAddress != user.EmailAddress)
            {
                if (!IsValidEmail(request.EmailAddress))
                {
                    throw new ArgumentException("Adresse email invalide.");
                }
                user.EmailAddress = request.EmailAddress;
                isUpdated = true;
            }

            if (request.ProfilePicture != null && request.ProfilePicture.Length > 0)
            {
                user.ProfilePicture = await SaveProfileImageAsync(request.ProfilePicture);
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(request.UpdatedPassword))
            {
                user.HashedPassword = AuthCommon.HashPassword(request.UpdatedPassword);
                isUpdated = true;
            }

            if (isUpdated)
            {
                await _userRepository.UpdateUserDataAsync(user);
            }

            // Mapper vers UserUpdateResult
            return _mapper.Map<UpdateUserProfileCommandResult>(user);
        }

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
