using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanLunch.Application.Auth
{
    public class LogoutCommand : IRequest<bool>
    {
        public string RefreshToken { get; set; }

        public LogoutCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
