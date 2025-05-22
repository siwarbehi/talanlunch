using MediatR;
using Microsoft.AspNetCore.Http;

namespace TalanLunch.Application.Users.Commands
{
    public class UpdateUserProfileCommand : IRequest<UpdateUserProfileCommandResult>
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? UpdatedPassword { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        public UpdateUserProfileCommand() { }

        public UpdateUserProfileCommand(int userId, string? firstName, string? lastName, string? phoneNumber, string? emailAddress, string? updatedPassword, IFormFile? profilePicture)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            UpdatedPassword = updatedPassword;
            ProfilePicture = profilePicture;
        }
    }
}
