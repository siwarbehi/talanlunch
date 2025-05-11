using MediatR;

namespace TalanLunch.Application.Auth
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
