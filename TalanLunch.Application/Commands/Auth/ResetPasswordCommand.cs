using MediatR;

namespace TalanLunch.Application.Commands.Auth
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public ResetPasswordCommand(string token, string newPassword)
        {
            Token = token;
            NewPassword = newPassword;
        }
    }
}
