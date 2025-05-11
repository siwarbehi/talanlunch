using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Common;

namespace TalanLunch.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByResetTokenAsync(request.Token);

            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
                return false;

            var hashedPassword = AuthCommon.HashPassword(request.NewPassword);

            user.HashedPassword = hashedPassword;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }
}
