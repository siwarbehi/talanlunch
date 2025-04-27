using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IMailService
    {

        Task SendEmailAsync(MailDataDto mailData);
        MailDataDto CreateMailDataForApproval(User caterer);
        Task SendPasswordResetEmailAsync(User user, string resetToken);
    }
}
