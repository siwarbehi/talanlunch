

namespace TalanLunch.Infrastructure.Models
{
    public class DishRating
    {
        public int RatingId { get; set; }  
        public int Rating { get; set; }
        public DateTime RatingDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; }


    }
}
