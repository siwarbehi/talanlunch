using System.ComponentModel.DataAnnotations;
using TalanLunch.Domain.Enums;


namespace TalanLunch.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;  
        public UserRole UserRole { get; set; }  
        public byte[]? ProfilePicture { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;  
        public string HashedPassword { get; set; } = string.Empty;  

       
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<DishRating> DishRatings { get; set; } = [];
    }
}
