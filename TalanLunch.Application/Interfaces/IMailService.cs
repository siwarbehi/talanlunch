using TalanLunch.Application.Dtos.Mail;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IMailService
    {

        Task SendEmailAsync(MailDataDto mailData);
        MailDataDto CreateMailDataForApproval(Domain.Entities.User caterer);
        Task SendPasswordResetEmailAsync(Domain.Entities.User user, string resetToken);
    }
}
