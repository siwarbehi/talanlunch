using Microsoft.AspNetCore.Http;

namespace TalanLunch.Application.Dtos.User
{
    public class UserDto
    {
      
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string EmailAddress { get; set; } = string.Empty;
            public string? UpdatedPassword { get; set; }

            public IFormFile? ProfilePicture { get; set; }
        

    }
}
