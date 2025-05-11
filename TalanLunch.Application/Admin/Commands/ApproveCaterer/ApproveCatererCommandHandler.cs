using MediatR;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Application.Admin.Commands.ApproveCaterer
{
    public class ApproveCatererCommandHandler : IRequestHandler<ApproveCatererCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public ApproveCatererCommandHandler(IUserRepository userRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task<bool> Handle(ApproveCatererCommand request, CancellationToken cancellationToken)
        {
            var caterer = await _userRepository.GetCatererByIdAsync(request.UserId);

            if (caterer == null || caterer.IsApproved)
                return false;

            caterer.IsApproved = true;

            bool updateSuccess = await _userRepository.ApproveUserAsync(caterer);
            if (!updateSuccess)
                return false;

            var mailData = _mailService.CreateMailDataForApproval(caterer);
            await _mailService.SendEmailAsync(mailData);

            return true;
        }
    }
}
