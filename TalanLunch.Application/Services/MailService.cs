using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TalanLunch.Application.Configurations;
using TalanLunch.Application.Dtos.Mail;
using TalanLunch.Application.Interfaces;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        private async Task SendMailAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_mailSettings.Server, _mailSettings.Port, false);
                    await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    // Vous pouvez remplacer par un mécanisme de log plus robuste ici
                    Console.WriteLine($"Erreur lors de l'envoi de l'e-mail: {ex.Message}");
                }
            }
        }

        private string BuildEmailBody(string bodyContent, string subject)
        {
            return $@"
                <!DOCTYPE html>
                <html lang='fr'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>{subject}</title>
                    <style>
                        body {{ font-family: 'Poppins', sans-serif; margin: 0; padding: 0; background-color: #f5f5f5; }}
                        .email-container {{ width: 100%; max-width: 600px; margin: 0 auto; background-color: #fff; padding: 30px; border-radius: 10px; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1); }}
                        .email-header {{ text-align: center; margin-bottom: 30px; }}
                        .email-header img {{ max-width: 150px; height: auto; }}
                        .email-body {{ color: #333; line-height: 1.6; }}
                        .email-footer {{ margin-top: 30px; text-align: center; font-size: 14px; color: #777; border-top: 1px solid #eee; padding-top: 20px; }}
                        .button {{ display: inline-block; padding: 10px 20px; background-color: #09619e; color: white !important; text-decoration: none; border-radius: 5px; font-weight: 600; margin: 15px 0; }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='email-header'>
                            <img src='https://votre-domaine.com/path/to/logo.png' alt='Logo de l'entreprise' />
                            <h2>{subject}</h2>
                        </div>
                        <div class='email-body'>
                            {bodyContent}
                        </div>
                        <div class='email-footer'>
                            <p>© 2025 Talan. Tous droits réservés.</p>
                            <p>Ceci est un email automatique, merci de ne pas y répondre.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        public async Task SendEmailAsync(MailDataDto mailData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(mailData.EmailToName, mailData.EmailToId));
            message.Subject = mailData.EmailSubject;

            string bodyContent = $@"
                Bonjour {mailData.EmailToName.Split(' ')[0]},<br/><br/>
                Votre demande d'inscription en tant que traiteur a été approuvée avec succès. <br/>
                Bienvenue sur notre plateforme.<br/><br/>
                <div class='motif'>
                    Pour toute question, veuillez contacter notre équipe via <a href='mailto:talantunsie@gmail.com' class='button'>Support</a>.
                </div>
                <br/>Cordialement,<br/>
                L'équipe Talan";

            message.Body = new TextPart("html") { Text = BuildEmailBody(bodyContent, mailData.EmailSubject) };

            await SendMailAsync(message);
        }

        public MailDataDto CreateMailDataForApproval(Domain.Entities.User caterer)
        {
            return new MailDataDto
            {
                EmailToId = caterer.EmailAddress,
                EmailToName = $"{caterer.FirstName} {caterer.LastName}",
                EmailSubject = "Confirmation d'Approbation de Traiteur",
            };
        }

        public async Task SendPasswordResetEmailAsync(Domain.Entities.User user, string resetToken)
        {
            string port = user.UserRole switch
            {
                UserRole.CATERER => "5173",
                UserRole.COLLABORATOR => "5174",
                _ => "5173"
            };

            string resetLink = $"http://localhost:{port}/reset-password?token={resetToken}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", user.EmailAddress));
            message.Subject = "🔒 Réinitialisation de votre mot de passe - Action requise";

            string bodyContent = $@"
                <p>Bonjour {user.FirstName},</p>
                <p>Nous avons reçu une demande de réinitialisation de votre mot de passe.</p>
                <p>Veuillez cliquer sur le bouton ci-dessous pour procéder :</p>
                <a class='button' href='{resetLink}'>Réinitialiser mon mot de passe</a>
                <p>Ou copiez ce lien dans votre navigateur :</p>
                <p><strong>{resetLink}</strong></p>
                <p>⚠️ Ce lien est valable pendant <strong>60 minutes</strong>.</p>";

            message.Body = new TextPart("html") { Text = BuildEmailBody(bodyContent, message.Subject) };

            await SendMailAsync(message);
        }
    }
}
