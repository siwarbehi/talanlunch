using MediatR;
using TalanLunch.Application.Helpers;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null) return false;

            string resetToken = Guid.NewGuid().ToString();
            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserAsync(user);

            string bodyContent = MailHelpers.ResetPasswordEmailFactory(user);
            string subject = "🔒 Réinitialisation de votre mot de passe - Action requise";

            await _mailService.SendEmailAsync(user.EmailAddress, user.FirstName, subject, bodyContent, cancellationToken)
                .ConfigureAwait(false);

            return true;
        }
    }
}
