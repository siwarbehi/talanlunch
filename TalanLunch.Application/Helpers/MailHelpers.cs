using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Application.Helpers
{
    static class MailHelpers
    {
        public static string ResetPasswordEmailFactory(User user)
        {
            string port = user.UserRole switch
            {
                UserRole.CATERER => "5173",
                UserRole.COLLABORATOR => "5174",
                _ => "5173"
            };

            string resetLink = $"http://localhost:{port}/reset-password?token={user.ResetToken}";

            string bodyContent = $@"
                <p>Bonjour {user.FirstName},</p>
                <p>Nous avons reçu une demande de réinitialisation de votre mot de passe.</p>
                <p>Veuillez cliquer sur le bouton ci-dessous pour procéder :</p>
                <a class='button' href='{resetLink}'>Réinitialiser mon mot de passe</a>
                <p>Ou copiez ce lien dans votre navigateur :</p>
                <p><strong>{resetLink}</strong></p>
                <p>⚠️ Ce lien est valable pendant <strong>60 minutes</strong>.</p>";

            return bodyContent;
        }

        public static string ApprouveEmailFactory(string name)
        {
            string bodyContent = $@"
                Bonjour {name},<br/><br/>
                Votre demande d'inscription en tant que traiteur a été approuvée avec succès. <br/>
                Bienvenue sur notre plateforme.<br/><br/>
                <div class='motif'>
                    Pour toute question, veuillez contacter notre équipe via <a href='mailto:talantunsie@gmail.com' class='button'>Support</a>.
                </div>
                <br/>Cordialement,<br/>
                L'équipe Talan";

            return bodyContent;
        }

    }
}
