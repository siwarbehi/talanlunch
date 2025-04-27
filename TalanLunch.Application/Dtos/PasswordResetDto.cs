using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Dtos
{
    public class PasswordResetDto
    {
        [Required]
        public string EmailToId { get; set; }
        [Required]
        public string EmailToName { get; set; }
        [Required]
        public string ResetLink { get; set; }
        [Required]
        public string EmailBody { get; set; } = string.Empty;
    }
}
