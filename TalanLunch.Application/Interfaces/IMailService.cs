using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Services;

namespace TalanLunch.Application.Interfaces
{
    public interface IMailService
    {

        Task SendEmailAsync(MailDataDto mailData);
        MailDataDto CreateMailDataForApproval(User caterer);
        Task SendPasswordResetEmailAsync(User user, string resetToken);
    }
}
