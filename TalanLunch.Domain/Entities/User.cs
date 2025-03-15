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
        public string EmailAddress { get; set; } = string.Empty;  
        public UserRole UserRole { get; set; }  
        public string? ProfilePicture { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;  
        public string HashedPassword { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false;


        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<DishRating> DishRatings { get; set; } = [];
    }
}
