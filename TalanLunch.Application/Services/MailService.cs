using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TalanLunch.Application.Configurations;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;
using TalanLunch.Application.Interfaces;
using MailKit.Net.Smtp;  
using MimeKit;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;  // Récupère les paramètres de configuration
        }

        public async Task SendEmailAsync(MailDataDto mailData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(mailData.EmailToName, mailData.EmailToId));
            message.Subject = mailData.EmailSubject;

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
                    Pour toute question, veuillez contacter notre équipe via <a href='mailto:talantunsie@gmail.com' class='button'>Support</a>.
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
        // Méthode pour envoyer un email de réinitialisation de mot de passe
        public async Task SendPasswordResetEmailAsync(User user, string resetToken)
        {
            // Choisir le port selon le rôle de l'utilisateur
            string port = user.UserRole switch
            {
                UserRole.CATERER => "5173",
                UserRole.COLLABORATOR => "5174",
                _ => "5173" // Par défaut, utiliser 5173
            };

            string resetLink = $"http://localhost:{port}/reset-password?token={resetToken}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress($"{user.FirstName} {user.LastName}", user.EmailAddress));
            message.Subject = "🔒 Réinitialisation de votre mot de passe - Action requise";

            string emailBody = $@"
<!DOCTYPE html>
<html lang='fr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Réinitialisation du mot de passe</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            padding: 20px;
            text-align: center;
        }}
        .container {{
            max-width: 600px;
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0px 4px 10px rgba(0,0,0,0.1);
            margin: auto;
        }}
        h2 {{
            color: #333;
        }}
        p {{
            font-size: 16px;
            color: #555;
        }}
        .button {{
            display: inline-block;
            background: #28a745;
            color: white;
            padding: 12px 25px;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            margin-top: 20px;
        }}
        .footer {{
            font-size: 12px;
            color: #888;
            margin-top: 20px;
            border-top: 1px solid #ddd;
            padding-top: 10px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>🔑 Réinitialisation de votre mot de passe</h2>
        <p>Bonjour {user.FirstName},</p>
        <p>Nous avons reçu une demande de réinitialisation de votre mot de passe.</p>
        <p>Veuillez cliquer sur le bouton ci-dessous pour procéder :</p>
        <a class='button' href='{resetLink}'>Réinitialiser mon mot de passe</a>
        <p>Ou copiez ce lien dans votre navigateur :</p>
        <p><strong>{resetLink}</strong></p>
        <p>⚠️ Ce lien est valable pendant <strong>60 minutes</strong>.</p>
        <div class='footer'>
            <p>Si vous n'avez pas demandé cette réinitialisation, ignorez cet email.</p>
            <p>© 2025 Talan. Tous droits réservés.</p>
        </div>
    </div>
</body>
</html>";

            message.Body = new TextPart("html") { Text = emailBody };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_mailSettings.Server, _mailSettings.Port, false);
                    client.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'envoi de l'e-mail: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }


    }
}
