﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TalanLunch.Application.Interfaces;

namespace TalanLunch.Infrastructure.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }


        public async Task SendEmailAsync(string toEmail, string toName, string subject, string body, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html") { Text = BuildEmailBody(body, subject) };

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

    }
}
