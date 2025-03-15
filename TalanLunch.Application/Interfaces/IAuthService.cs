using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.DTOs;

namespace TalanLunch.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterUserDto registerUserDto, bool isCaterer);

    }
}
