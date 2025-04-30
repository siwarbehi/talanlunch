using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@talan\.[a-zA-Z]{2,}$", ErrorMessage = "L'email doit contenir 'talan' dans le domaine.")]
        public string EmailAddress { get; set; } = string.Empty;

        public UserRole UserRole { get; set; }

        public string? ProfilePicture { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string HashedPassword { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<DishRating> DishRatings { get; set; } = new List<DishRating>();
    }
}
