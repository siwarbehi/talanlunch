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
            // Utilisation du corps par défaut avec le prénom dynamique
            string emailBody = $"Bonjour {mailData.EmailToName.Split(' ')[0]},<br/><br/>" +
                   "Votre demande d'inscription en tant que traiteur a été approuvée avec succès. " +
                   "Bienvenue sur notre plateforme.<br/><br/>Cordialement,<br/>L'équipe Talan.";

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
            // Vérification que le prénom n'est pas nul ou vide
            string firstName = string.IsNullOrEmpty(caterer.FirstName) ? "Cher Traiteur" : caterer.FirstName;

            // Modèle de message avec interpolation de chaîne
            string emailBody = $@"
        Bonjour {firstName}, 

        Votre demande d'inscription en tant que traiteur a été approuvée avec succès. 
        Bienvenue sur notre plateforme.

        Cordialement,
        L'équipe Talan";

            return new MailDataDto
            {
                EmailToId = caterer.EmailAddress,
                EmailToName = $"{caterer.FirstName} {caterer.LastName}",
                EmailSubject = "Confirmation d'Approbation de Traiteur",
            
            };
        }

    }
}
