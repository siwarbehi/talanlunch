namespace TalanLunch.Infrastructure.Mail
{
    public class MailSettings
    {
        public required string SenderName { get; set; }
        public required string Server { get; set; }
        public required string SenderEmail { get; set; }
        public int Port { get; set; }

        public required string UserName { get; set; }
        public required string Password { get; set; }

    }
}
