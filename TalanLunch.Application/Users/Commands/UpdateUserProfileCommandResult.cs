using Microsoft.AspNetCore.Http;

namespace TalanLunch.Application.Users.Commands
{
    public class UpdateUserProfileCommandResult
    {

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }

        public string? ProfilePicture { get; set; }// ici c’est l'URL Azure


    }
}
