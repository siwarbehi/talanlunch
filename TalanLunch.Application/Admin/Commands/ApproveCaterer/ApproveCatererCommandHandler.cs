using MediatR;
using TalanLunch.Application.Interfaces;
using TalanLunch.Application.Helpers;

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

            string bodyContent = MailHelpers.ApprouveEmailFactory(caterer.FirstName);

            await _mailService.SendEmailAsync(caterer.EmailAddress, $"{caterer.FirstName} {caterer.LastName}", "Confirmation d'Approbation de Traiteur", bodyContent, cancellationToken);

            return true;
        }
    }
}
