using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Application.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<string>
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
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Le numéro de téléphone doit contenir uniquement des chiffres.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsCaterer { get; set; }

        public RegisterUserCommand() { }

        public RegisterUserCommand(string firstName, string lastName, string email, string password, string phone, bool isCaterer)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = email;
            Password = password;
            PhoneNumber = phone;
            IsCaterer = isCaterer;
        }
    }
}
