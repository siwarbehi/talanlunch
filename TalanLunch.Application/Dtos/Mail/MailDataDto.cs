using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Dtos.Mail
{
    public class MailDataDto
    {
        [Required]
        [EmailAddress]
        public string EmailToId { get; set; } = string.Empty;

        [Required]
        public string EmailToName { get; set; } = string.Empty;

        [Required]
        public string EmailSubject { get; set; } = string.Empty;

        [Required]
        public string EmailBody { get; set; } = string.Empty;
    }
}
