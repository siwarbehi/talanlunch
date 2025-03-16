using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TalanLunch.Application.Configurations;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Interfaces;
using MailKit.Net.Smtp;  // Utilisation de MailKit
using MimeKit;

namespace TalanLunch.Application.Services
{
    public class TalanMailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public TalanMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;  // Récupère les paramètres de configuration
        }

        public async Task SendEmailAsync(MailDataDto mailData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(mailData.EmailToName, mailData.EmailToId));
            message.Subject = mailData.EmailSubject;

            // Contenu de l'email avec le HTML et le style
            string emailBody = $@"
    <!DOCTYPE html>
    <html lang='fr'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>{mailData.EmailSubject}</title>
        <style>
            body {{
                font-family: 'Poppins', sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f5f5f5;
            }}
            .email-container {{
                width: 100%;
                max-width: 600px;
                margin: 0 auto;
                background-color: #fff;
                padding: 30px;
                border-radius: 10px;
                box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
            }}
            .email-header {{
                text-align: center;
                margin-bottom: 30px;
            }}
            .email-header img {{
                max-width: 150px;
                height: auto;
            }}
            .email-body {{
                color: #333;
                line-height: 1.6;
            }}
            .email-footer {{
                margin-top: 30px;
                text-align: center;
                font-size: 14px;
                color: #777;
                border-top: 1px solid #eee;
                padding-top: 20px;
            }}
            .button {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #09619e;
                color: white !important;
                text-decoration: none;
                border-radius: 5px;
                font-weight: 600;
                margin: 15px 0;
            }}
            .motif {{
                background-color: #f8f9fa;
                border-left: 4px solid #09619e;
                padding: 10px 15px;
                margin: 15px 0;
            }}
            .highlight {{
                color: #e9812c;
                font-weight: bold;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                <img src='https://votre-domaine.com/path/to/logo.png' alt='Logo de l'entreprise' />
                <h2>{mailData.EmailSubject}</h2>
            </div>
            <div class='email-body'>
                Bonjour {mailData.EmailToName.Split(' ')[0]},<br/><br/>
                Votre demande d'inscription en tant que traiteur a été approuvée avec succès. <br/>
                Bienvenue sur notre plateforme.<br/><br/>
                <div class='motif'>
                    Pour toute question, veuillez contacter notre équipe via <a href='https://www.talan.com/global/fr' class='button'>Support</a>.
                </div>
                <br/>Cordialement,<br/>
                L'équipe Talan
            </div>
            <div class='email-footer'>
                <p>© 2025 Talan. Tous droits réservés.</p>
                <p>Ceci est un email automatique, merci de ne pas y répondre.</p>
            </div>
        </div>
    </body>
    </html>";

            // Affectation du corps du message en HTML
            message.Body = new TextPart("html") { Text = emailBody };

            // Connexion et envoi avec MailKit
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_mailSettings.Server, _mailSettings.Port, false);
                    client.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'envoi de l'e-mail: {ex.Message}");
                }
                finally
                {
                    // Déconnexion après l'envoi
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }


        public MailDataDto CreateMailDataForApproval(User caterer)
        {
         
            return new MailDataDto
            {
                EmailToId = caterer.EmailAddress,
                EmailToName = $"{caterer.FirstName} {caterer.LastName}",
                EmailSubject = "Confirmation d'Approbation de Traiteur",
            
            };
        }

    }
}
