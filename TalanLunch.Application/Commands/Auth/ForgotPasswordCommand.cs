using MediatR;

namespace TalanLunch.Application.Commands.Auth
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }

        public ForgotPasswordCommand(string email)
        {
            Email = email;
        }
    }
}
