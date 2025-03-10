using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Domain.Entities;
namespace TalanLunch.Application.DTOs
{
    public class LoginDto
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
