using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Dtos.Auth
{
    public class RegisterUserDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Le prénom ne peut contenir que des lettres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Le nom de famille ne peut contenir que des lettres.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Le numéro de téléphone doit contenir uniquement des chiffres.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
