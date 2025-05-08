using MediatR;
using TalanLunch.Application.Commands.Auth;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Handlers.Auth
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

            await _mailService.SendPasswordResetEmailAsync(user, resetToken);
            return true;
        }
    }
}
