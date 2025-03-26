using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Dtos
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
