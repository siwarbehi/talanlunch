

using System.ComponentModel.DataAnnotations;

namespace TalanLunch.Domain.Entities
{
    public class DishRating
    {
        public int RatingId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public DateTime RatingDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }

    
        public required User User { get; set; }

    }
}
